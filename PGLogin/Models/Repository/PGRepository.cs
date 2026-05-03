using Microsoft.EntityFrameworkCore;
using PGLogin.Models.Data;
using PGLogin.Models.DTO;
using PGLogin.Models.Repository.IRepository;
using System;

namespace PGLogin.Models.Repository
{
    public class PGRepository : IPGRepository
    {
        private readonly MydBContext _mydBContext;
        public PGRepository(MydBContext mydBContext)
        {
            _mydBContext = mydBContext;
        }
        public async Task<PG> AddPG(PGDTO pgDTO)
        {
            var result = new PG
            {
                PG_Name = pgDTO.PG_Name,
                AreaId = (Guid)pgDTO.AreaId,
                Price = pgDTO.Price,
                SharingType = pgDTO.SharingType,
                ImagePath = pgDTO.ImagePath,
            };
            await _mydBContext.pGs.AddAsync(result);
            await _mydBContext.SaveChangesAsync();
            return result;
        }

        public async Task<PG> DeletePG(Guid Pg_Id)
        {
            var result = await _mydBContext.pGs.FirstOrDefaultAsync(x => x.PG_Id == Pg_Id);
            if(result != null)
            {
                _mydBContext.pGs.Remove(result);
                await _mydBContext.SaveChangesAsync();    

            }
            return result;
        }

        public async Task<IEnumerable<PG>> GetAllPGs()
        {
            return await _mydBContext.pGs.ToListAsync();
        }

        public async Task<PG> GetPGById(Guid Pg_Id)
        {
            var result = await _mydBContext.pGs.FirstOrDefaultAsync(x => x.PG_Id == Pg_Id);
            return result;
        }

        public async Task<List<PG>> Search_PG(PGDTO filter)
        {
            var query = _mydBContext.pGs
                .Include(p => p.Area)
                .ThenInclude(a => a.City)
                .AsQueryable();

            // Filter by Area
            if (filter.AreaId != Guid.Empty)
                query = query.Where(p => p.AreaId == filter.AreaId);

            // Filter by Price (<=)
            if (filter.Price.HasValue)
                query = query.Where(p => p.Price <= filter.Price.Value);

            // Filter by Sharing Type
            if (filter.SharingType.HasValue)
                query = query.Where(p => p.SharingType == filter.SharingType.Value);

            // Filter by PG Name
            if (!string.IsNullOrWhiteSpace(filter.PG_Name))
                query = query.Where(p => p.PG_Name.Contains(filter.PG_Name));

            // Return the matching PG list
            return await query.ToListAsync();
        }

        public async Task<PG> UpdatePG(PG pgDTO)
        {
            var result = await _mydBContext.pGs.FirstOrDefaultAsync(x => x.PG_Id == pgDTO.PG_Id);
            if(result != null)
            {
                result.AreaId = pgDTO.AreaId;
                result.PG_Name = pgDTO.PG_Name;
                result.Price = pgDTO.Price;
                result.SharingType = pgDTO.SharingType;
                result.ImagePath = pgDTO.ImagePath;
            }
            return result;
        }
    }
}
