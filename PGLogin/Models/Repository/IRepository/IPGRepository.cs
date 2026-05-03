using PGLogin.Models.DTO;

namespace PGLogin.Models.Repository.IRepository
{
    public interface IPGRepository
    {
        Task<PG> GetPGById(Guid Pg_Id);
        Task<IEnumerable<PG>> GetAllPGs();
        Task<PG> AddPG(PGDTO pgDTO);
        Task<PG> UpdatePG(PG pgDTO);
        Task<PG> DeletePG(Guid Pg_Id);
        Task<List<PG>> Search_PG(PGDTO filter);
    }
}
