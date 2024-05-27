
namespace BaseLibrary.Responses
{
    public record LoginResponse(bool flag,string Message = null!,string Token = null!,string RefreshToken = null!);
}
