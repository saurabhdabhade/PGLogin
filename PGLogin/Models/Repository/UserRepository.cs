using Microsoft.EntityFrameworkCore;
using PGLogin.Models.Data;
using PGLogin.Models.DTO;
using PGLogin.Models.Repository.IRepository;

namespace PGLogin.Models.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MydBContext _mydBContext;
        public UserRepository(MydBContext mydBContext)
        {
            _mydBContext = mydBContext;
        }
        public async Task<User> AddUser(UserDTO userDTO)
        {
            var result = new User
            {
                User_Name = userDTO.User_Name,
                User_Email = userDTO.User_Email,
                Role_Id = userDTO.Role_Id,
            };
            await _mydBContext.users.AddAsync(result);
            await _mydBContext.SaveChangesAsync();
            return result;
        }

        public async Task<User> DeleteUser(Guid user_Id)
        {
            var result = await _mydBContext.users.FirstOrDefaultAsync(x => x.User_Id == user_Id);
            if(result != null)
            {
                _mydBContext.users.Remove(result);  
                await _mydBContext.SaveChangesAsync();    
            }
            return result;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _mydBContext.users.ToListAsync();
        }

        public async Task<User> GetUserById(Guid user_Id)
        {
            var result = await _mydBContext.users.FirstOrDefaultAsync(x => x.User_Id == user_Id);
            return result;
        }

        public async Task<User> UpdateUser(User userDTO)
        {
            var result = await _mydBContext.users.FirstOrDefaultAsync(x => x.User_Id == userDTO.User_Id);
            if(result != null)
            {
                result.User_Name = userDTO.User_Name;
                result.User_Email = userDTO.User_Email;
                result.Role_Id = userDTO.Role_Id;
            }
            return result;
        }
    }
}
