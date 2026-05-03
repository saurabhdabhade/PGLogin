using PGLogin.Models.DTO;

namespace PGLogin.Models.Repository.IRepository
{
    public interface IRole_Page_Mapper_Repository
    {
        Task<Role_Page_Mapper> GetRole_Page_MapperId(Guid role_Page_Mapper_Id);
        Task<IEnumerable<Role_Page_Mapper>> GetAllRole_Page_Mappers();
        Task<Role_Page_Mapper> AddRole_Page_Mapper(Role_Page_MapperDTO role_Page_MapperDTO);
        Task<Role_Page_Mapper> UpdateRole_Page_Mapper(Role_Page_Mapper role_Page_MapperDTO);
        Task<Role_Page_Mapper> DeleteRole_Page_Mapper(Guid role_Page_Mapper_Id);
    }
}
