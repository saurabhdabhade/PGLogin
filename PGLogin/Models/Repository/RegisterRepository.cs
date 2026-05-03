using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using PGLogin.Models.Data;
using PGLogin.Models.DTO;
using PGLogin.Models.Repository.IRepository;
using System;
using System.Security.AccessControl;

namespace PGLogin.Models.Repository
{
    public class RegisterRepository : IRegisterRepository
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IHttpContextAccessor _context;
        private readonly MydBContext _dbContext;
        private readonly IServiceRepository _serviceRepository;  
        private string registerUrl;
        public RegisterRepository(MydBContext dbContext, IHttpClientFactory clientFactory, IConfiguration configuration, IHttpContextAccessor context, IServiceRepository serviceRepository  ) : base()
        {
            _dbContext = dbContext;
            _context = context;
            _clientFactory = clientFactory;
            registerUrl = configuration.GetValue<string>("ServiceUrls:Admin");
            _serviceRepository = serviceRepository;
        }
        public async Task<Register> Delete(int RegisterID)
        {
            var result = await _dbContext.registers.FirstOrDefaultAsync(x => x.RegisterID == RegisterID);
            if (result != null)
            {
                _dbContext.registers.Remove(result);
                await _dbContext.SaveChangesAsync();
            }
            return result;
        }

        public async Task<Register> Get(int RegisterID)
        {
            return await _dbContext.registers.FirstOrDefaultAsync(u => u.RegisterID == RegisterID);
        }

        public async Task<IEnumerable<Register>> GetAll()
        {
            return await _dbContext.registers.ToListAsync();
        }

        public Task<Register> Login<Register>(RegisterDTO registerDTOs)
        {
            return _serviceRepository.SendAsync<Register>(new APIRequest()
            {
                ApiType = "Post",
                Data = registerDTOs,
                Url = registerUrl + "RegisterController/Logins"
            });
        }

        public async Task<Register> Registers(RegisterDTO register)
        {
            var registers = new Register
            {
                First_Name = register.First_Name,
                Last_Name = register.Last_Name,
                Email = register.Email,
                Password = register.Password,
                Confirm_Password = register.Confirm_Password,
                LastPassword1 = register.Password,
                LastPassword2 = register.Password
            };

            await _dbContext.registers.AddAsync(registers);
            await _dbContext.SaveChangesAsync();

            return await Task.FromResult(registers);
        }

        public async Task<Register> Update(Register register)
        {
            var result = await _dbContext.registers.FirstOrDefaultAsync(u => u.RegisterID == register.RegisterID);
            if (result != null)
            {
                result.RegisterID = register.RegisterID;
                result.First_Name = register.First_Name;
                result.Last_Name = register.Last_Name;
                result.Password = register.Password;
                result.Email = register.Email;
                result.Confirm_Password = register.Confirm_Password;
                result.LastPassword1 = register.LastPassword1;
                result.LastPassword2 = register.LastPassword2;
                result.IsDeleted = register.IsDeleted;
                await _dbContext.SaveChangesAsync();
                return result;
            }
            return null;
        }

        Task<Register> IRegisterRepository.Token_Call<Register>(RegisterDTO registerDTOs)
        {
            return _serviceRepository.SendAsync<Register>(new APIRequest()
            {
                ApiType = "Post",
                Data = registerDTOs,
                Url = registerUrl + "RegisterController/Logins"
            });
        }
    }
}
