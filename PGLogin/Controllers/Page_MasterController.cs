using Microsoft.AspNetCore.Mvc;
using PGLogin.Models;
using PGLogin.Models.Data;
using PGLogin.Models.DTO;
using PGLogin.Models.Repository.IRepository;

namespace PGLogin.Controllers
{
    public class Page_MasterController : Controller
    {
        private readonly IPage_MasterRepository _page_MasterRepository;
        private readonly MydBContext _context;
        public Page_MasterController(IPage_MasterRepository page_MasterRepository, MydBContext mydBContext)
        {
            _page_MasterRepository = page_MasterRepository;
            _context = mydBContext;
        }
        public IActionResult GetsPage_Masters()
        {
            var result = _context.page_Masters.ToList();
            return View(result);
        }
        public async Task<IActionResult> GetsPageMasters()
        {
            var result = await _page_MasterRepository.GetAllPage_Masters();
            return View("GetsPage_Masters", result);
        }
        public IActionResult CreatePage_Master()
        {
            return View();
        }
        public async Task<IActionResult> CreatePageMasters(Page_Master page_Master)
        {
            var result = new Page_MasterDTO()
            {
                Page_Name = page_Master.Page_Name,
                IsActive = page_Master.IsActive,
            };
            await _page_MasterRepository.AddPage_Master(result);
            return View("GetsPage_Masters", "Page_Master");
        }
        public IActionResult EditPage_Master(Guid id)
        {
            var result = _page_MasterRepository.GetPage_MasterId(id);
            return View(result);
        }
        public async Task<IActionResult> EditPageMasters(Page_Master page_Master)
        {
            var result = await _page_MasterRepository.UpdatePage_Master(page_Master);
            if (result == null)
            {
                TempData["SuccessMessage"] = "Page Master Edited Successfully!...";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Edit Page Master!...";
            }
            return RedirectToAction("GetsPage_Masters", "Page_Master");
        }
        public async Task<IActionResult> DeletePage_Master(Guid id)
        {
            await _page_MasterRepository.DeletePage_Master(id);
            return RedirectToAction("GetsPage_Masters", "Page_Master");
        }
    }
}
