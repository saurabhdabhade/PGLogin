using PGLogin.Models.DTO;

namespace PGLogin.Models.Repository.IRepository
{
    public interface IRole_MasterRepository
    {
        Task<Role_Master> GetRole_MasterById(Guid Role_Master_Id);
        Task<IEnumerable<Role_Master>> GetAllRole_Masters();
        Task<Role_Master> AddRole_Master(Role_MasterDTO role_MasterDTO);
        Task<Role_Master> UpdateRole_Master(Role_Master role_MasterDTO);
        Task<Role_Master> DeleteRole_Master(Guid Role_Master_Id);
    }
}
