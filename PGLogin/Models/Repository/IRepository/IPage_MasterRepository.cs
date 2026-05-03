using PGLogin.Models.DTO;

namespace PGLogin.Models.Repository.IRepository
{
    public interface IPage_MasterRepository
    {
        Task<Page_Master> GetPage_MasterId(Guid Page_Master_Id);
        Task<IEnumerable<Page_Master>> GetAllPage_Masters();
        Task<Page_Master> AddPage_Master(Page_MasterDTO Page_MasterDTO);
        Task<Page_Master> UpdatePage_Master(Page_Master Page_MasterDTO);
        Task<Page_Master> DeletePage_Master(Guid Page_Master_Id);
    }
}
