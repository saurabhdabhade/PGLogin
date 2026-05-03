using Microsoft.EntityFrameworkCore;
using PGLogin.Models.Data;
using PGLogin.Models.DTO;
using PGLogin.Models.Repository.IRepository;

namespace PGLogin.Models.Repository
{
    public class AreaRepository : IAreaRepository
    {
        private readonly MydBContext _mydBContext;
        public AreaRepository(MydBContext mydBContext)
        {
            _mydBContext = mydBContext;
        }
        public async Task<Area> AddArea(AreaDTO areaDTO)
        {
            var result = new Area
            {
                AreaName = areaDTO.AreaName,
                City_Id = areaDTO.City_Id,
            };
            await _mydBContext.areas.AddAsync(result);
            await _mydBContext.SaveChangesAsync();
            return result;
        }

        public async Task<Area> DeleteArea(Guid Area_Id)
        {
            var result = await _mydBContext.areas.FirstOrDefaultAsync(x => x.AreaId == Area_Id);
            if(result != null)
            {
                _mydBContext.Remove(result);
                await _mydBContext.SaveChangesAsync();
            }
            return result;
        }

        public async Task<IEnumerable<Area>> GetAllAreas()
        {
            return await _mydBContext.areas.ToListAsync();
        }

        public async Task<Area> GetAreaById(Guid Area_Id)
        {
            var result = await _mydBContext.areas.FirstOrDefaultAsync(x => x.AreaId == Area_Id);
            return result;
        }

        public async Task<Area> UpdateArea(Area areaDTO)
        {
            var result = await _mydBContext.areas.FirstOrDefaultAsync(x => x.AreaId == areaDTO.AreaId);
            if(result != null)
            {
                result.AreaName = areaDTO.AreaName;
                result.City_Id  = areaDTO.City_Id;
            }
            return result;
        }
    }
}
