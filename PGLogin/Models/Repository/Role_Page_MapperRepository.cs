using Microsoft.EntityFrameworkCore;
using PGLogin.Models.Data;
using PGLogin.Models.DTO;
using PGLogin.Models.Repository.IRepository;

namespace PGLogin.Models.Repository
{
    public class Role_Page_MapperRepository : IRole_Page_Mapper_Repository
    {
        private readonly MydBContext _mydBContext;
        public Role_Page_MapperRepository(MydBContext mydBContext)
        {
            _mydBContext = mydBContext;
        }
        public async Task<Role_Page_Mapper> AddRole_Page_Mapper(Role_Page_MapperDTO role_Page_MapperDTO)
        {
            var result = new Role_Page_Mapper
            {
                Role_Id = role_Page_MapperDTO.Role_Id,
                Page_Id = role_Page_MapperDTO.Page_Id,
                IsActive = role_Page_MapperDTO.IsActive,  
            };
            await _mydBContext.role_Page_Mappers.AddAsync(result);    
            await _mydBContext.SaveChangesAsync();
            return result;
        }

        public async Task<Role_Page_Mapper> DeleteRole_Page_Mapper(Guid role_Page_Mapper_Id)
        {
            var result = await _mydBContext.role_Page_Mappers.FirstOrDefaultAsync(x => x.Role_Page_ID == role_Page_Mapper_Id);
            if(result != null)
            {
                _mydBContext.role_Page_Mappers.Remove(result);
                await _mydBContext.SaveChangesAsync();
            }
            return result;
        }

        public async Task<IEnumerable<Role_Page_Mapper>> GetAllRole_Page_Mappers()
        {
            return await _mydBContext.role_Page_Mappers.ToListAsync();
        }

        public async Task<Role_Page_Mapper> GetRole_Page_MapperId(Guid role_Page_Mapper_Id)
        {
            var result = await _mydBContext.role_Page_Mappers.FirstOrDefaultAsync(x => x.Role_Page_ID == role_Page_Mapper_Id);
            return result;
        }

        public async Task<Role_Page_Mapper> UpdateRole_Page_Mapper(Role_Page_Mapper role_Page_MapperDTO)
        {
            var result = await _mydBContext.role_Page_Mappers.FirstOrDefaultAsync(x => x.Role_Page_ID == role_Page_MapperDTO.Role_Page_ID);
            if(result != null)
            {
                result.Role_Id = role_Page_MapperDTO.Role_Id;
                result.Page_Id = role_Page_MapperDTO.Page_Id;
                result.IsActive = role_Page_MapperDTO.IsActive;
            }
            return result;
        }
    }
}
