using Microsoft.AspNetCore.Identity.Data;
using PGLogin.Models.DTO;

namespace PGLogin.Models.Repository.IRepository
{
    public interface IRegisterRepository
    {
        Task<Register> Registers(RegisterDTO register);
        Task<IEnumerable<Register>> GetAll();
        Task<Register> Get(int RegisterID);
        Task<Register> Update(Register register);
        Task<Register> Delete(int RegisterID);
        Task<Register> Login<Register>(RegisterDTO registerDTOs);
        Task<Register> Token_Call<Register>(RegisterDTO registerDTOs);
    }
}
