using PGLogin.Models.DTO;

namespace PGLogin.Models.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<User> GetUserById(Guid user_Id);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> AddUser(UserDTO userDTO);
        Task<User> UpdateUser(User userDTO);
        Task<User> DeleteUser(Guid user_Id);
    }
}
