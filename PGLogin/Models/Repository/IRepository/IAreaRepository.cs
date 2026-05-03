using PGLogin.Models.DTO;

namespace PGLogin.Models.Repository.IRepository
{
    public interface IAreaRepository
    {
        Task<Area> GetAreaById(Guid Area_Id);
        Task<IEnumerable<Area>> GetAllAreas();
        Task<Area> AddArea(AreaDTO areaDTO);
        Task<Area> UpdateArea(Area areaDTO);
        Task<Area> DeleteArea(Guid Area_Id);
    }
}
