using Microsoft.EntityFrameworkCore;
using PGLogin.Models.Data;
using PGLogin.Models.DTO;
using PGLogin.Models.Repository.IRepository;

namespace PGLogin.Models.Repository
{
    public class Page_MasterRepository : IPage_MasterRepository
    {
        private readonly MydBContext _mydBContext;
        public Page_MasterRepository(MydBContext mydBContext)
        {
            _mydBContext = mydBContext;
        }
        public async Task<Page_Master> AddPage_Master(Page_MasterDTO Page_MasterDTO)
        {
            var result = new Page_Master
            {
                Page_Name = Page_MasterDTO.Page_Name,
                IsActive = Page_MasterDTO.IsActive,
            };
            await _mydBContext.page_Masters.AddAsync(result);
            await _mydBContext.SaveChangesAsync();
            return result;
        }

        public async Task<Page_Master> DeletePage_Master(Guid Page_Master_Id)
        {
            var result = await _mydBContext.page_Masters.FirstOrDefaultAsync(x => x.Page_Id == Page_Master_Id);
            if(result != null)
            {
                _mydBContext.page_Masters.Remove(result);
                await _mydBContext.SaveChangesAsync();    
            }
            return result;
        }

        public async Task<IEnumerable<Page_Master>> GetAllPage_Masters()
        {
            return await _mydBContext.page_Masters.ToListAsync();
        }

        public async Task<Page_Master> GetPage_MasterId(Guid Page_Master_Id)
        {
            var result = await _mydBContext.page_Masters.FirstOrDefaultAsync(x => x.Page_Id == Page_Master_Id);
            return result;     
        }

        public async Task<Page_Master> UpdatePage_Master(Page_Master Page_MasterDTO)
        {
            var result = await _mydBContext.page_Masters.FirstOrDefaultAsync(x => x.Page_Id == Page_MasterDTO.Page_Id);
            if(result != null)
            {
                result.Page_Name = Page_MasterDTO.Page_Name;
                result.IsActive = Page_MasterDTO.IsActive;
            }
            return result;
        }
    }
}
