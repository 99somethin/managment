﻿
using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Contracts;

namespace ServerLibrary.Repositories.Implementations
{
    public class TownRepository(AppDbContext appDbContext) : IGenericRepositoryInterface<Town>
    {
        
            public async Task<GeneralResponse> DeleteById(int id)
            {
                var db = await appDbContext.Towns.FindAsync(id);
                if (db is null) return NotFound();

                appDbContext.Towns.Remove(db);
                await Commit();
                return Success();
            }

            public async Task<List<Town>> GetAll() => await appDbContext.Towns.ToListAsync();


            public async Task<Town> GetById(int id) => await appDbContext.Towns.FindAsync(id);


            public async Task<GeneralResponse> Insert(Town item)
            {
                if (!await CheckName(item.Name!)) return new GeneralResponse(false, "already added");
                appDbContext.Towns.Add(item);

                await Commit();
                return Success();
            }

            public async Task<GeneralResponse> Update(Town item)
            {
                var dep = await appDbContext.Towns.FindAsync(item.Id);
                if (dep is null) return NotFound();
                dep.Name = item.Name;
                await Commit();
                return Success();
            }

            private static GeneralResponse NotFound() => new(false, "Departmen not found");

            private static GeneralResponse Success() => new(false, "Completed");

            private async Task Commit() => await appDbContext.SaveChangesAsync();

            private async Task<bool> CheckName(string name)
            {
                var item = await appDbContext.Towns.FirstOrDefaultAsync(x => x.Name!.ToLower().Equals(name.ToLower()));
                return item is null;
            }
        
    }
}
