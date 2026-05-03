using Microsoft.EntityFrameworkCore;
using PGLogin.Models.Data;
using PGLogin.Models.DTO;
using PGLogin.Models.Repository.IRepository;

namespace PGLogin.Models.Repository
{
    public class Role_MasterRepository : IRole_MasterRepository
    {
        private readonly MydBContext _mydBContext;
        public Role_MasterRepository(MydBContext mydBContext)
        {
            _mydBContext = mydBContext;
        }

        public async Task<Role_Master> AddRole_Master(Role_MasterDTO role_MasterDTO)
        {
            var result = new Role_Master
            {
                User = role_MasterDTO.User, 
                Admin = role_MasterDTO.Admin,
                IsActive = role_MasterDTO.IsActive,
                CreatedAt = role_MasterDTO.CreatedAt,
                UpdatedAt = role_MasterDTO.UpdatedAt,
            };
            await _mydBContext.role_Masters.AddAsync(result);
            await _mydBContext.SaveChangesAsync();
            return result;
        }

        public async Task<Role_Master> DeleteRole_Master(Guid Role_Master_Id)
        {
            var result = await _mydBContext.role_Masters.FirstOrDefaultAsync(x => x.Role_Id == Role_Master_Id);
            if (result != null)
            {
                _mydBContext.role_Masters.Remove(result);
                await _mydBContext.SaveChangesAsync();    
            }
            return result;
        }

        public async Task<IEnumerable<Role_Master>> GetAllRole_Masters()
        {
            return await _mydBContext.role_Masters.ToListAsync();
        }

        public async Task<Role_Master> GetRole_MasterById(Guid Role_Master_Id)
        {
            var result = await _mydBContext.role_Masters.FirstOrDefaultAsync(x => x.Role_Id == Role_Master_Id);
            return result;
        }

        public async Task<Role_Master> UpdateRole_Master(Role_Master role_MasterDTO)
        {
            var result = await _mydBContext.role_Masters.FirstOrDefaultAsync(x => x.Role_Id == role_MasterDTO.Role_Id);
            if (result != null)
            {
                result.User = role_MasterDTO.User;
                result.Admin = role_MasterDTO.Admin;
                result.IsActive = role_MasterDTO.IsActive;
                result.CreatedAt = role_MasterDTO.CreatedAt;
                result.UpdatedAt = role_MasterDTO.UpdatedAt;
            }
            return result;
        }
    }
}
