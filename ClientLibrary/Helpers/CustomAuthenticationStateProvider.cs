using BaseLibrary.DTOs;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace ClientLibrary.Helpers
{
    public class CustomAuthenticationStateProvider(LocalStorageService localStorageService) : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal anonymus = new(new ClaimsIdentity());

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var stringToken = await localStorageService.GetToken();
            if(string.IsNullOrEmpty(stringToken)) return await Task.FromResult(new AuthenticationState(anonymus));

            var deserializeToken = Serialization.DeserializeJsonString<UserSession>(stringToken);
            if (deserializeToken == null) return await Task.FromResult(new AuthenticationState(anonymus));

            var getUserClaims = DecryptToken(deserializeToken.Token!);
            if (getUserClaims == null) return await Task.FromResult(new AuthenticationState(anonymus));

            var claimsPrincipal = SetClaimPrincipial(getUserClaims);
            return await Task.FromResult(new AuthenticationState(claimsPrincipal));
        }

        public async Task UpdateAuthenticationState(UserSession userSession)
        {
            var claimsPrincipal = new ClaimsPrincipal();

            if (userSession.Token != null || userSession.RefreshToken != null)
            {
                var serializeSession = Serialization.SerializeObject(userSession);
                await localStorageService.SetToken(serializeSession);

                var getUserClaims = DecryptToken(userSession.Token);
                claimsPrincipal = SetClaimPrincipial(getUserClaims);
            }
            else
            {
                await localStorageService.RemoveToken();
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }



        public static ClaimsPrincipal SetClaimPrincipial(CustomUserClaims claims)
        {
            if (claims.Email is null)
                return new ClaimsPrincipal();

            var claimsList = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, claims.Id),
        new Claim(ClaimTypes.Name, claims.Name),
        new Claim(ClaimTypes.Email, claims.Email),
        new Claim(ClaimTypes.Role, claims.Role)
    };

            var identity = new ClaimsIdentity(claimsList, "JwtAuth");
            return new ClaimsPrincipal(identity);
        }



        private static CustomUserClaims DecryptToken(string jwtToken)
        {
            if (string.IsNullOrEmpty(jwtToken))
                return new CustomUserClaims();

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(jwtToken);

            var userId = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var name = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var email = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var role = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            return new CustomUserClaims(userId, name, email, role);
        }
    }
}
