using Microsoft.AspNetCore.Mvc;
using PGLogin.Models;
using PGLogin.Models.Data;
using PGLogin.Models.DTO;
using PGLogin.Models.Repository.IRepository;

namespace PGLogin.Controllers
{
    public class Role_MasterController : Controller
    {
        private readonly IRole_MasterRepository _role_MasterRepository;
        private readonly MydBContext _mydBContext;

        public Role_MasterController(IRole_MasterRepository role_MasterRepository, MydBContext mydBContext)
        {
            _role_MasterRepository = role_MasterRepository;
            _mydBContext = mydBContext;
        }
        public IActionResult GetsAllRole_Masters()
        {
            var result = _mydBContext.role_Masters.ToList();
            return View(result);
        }
        public async Task<IActionResult> GetsAllRoleMasters()
        {
            var result = await _role_MasterRepository.GetAllRole_Masters();
            return View("GetsAllRole_Masters", result);
        }
        public IActionResult CreateRole_Masters()
        {
            return View();
        }
        public async Task<IActionResult> CreateRoleMasters(Role_Master role_Master)
        {
            var result = new Role_MasterDTO()
            {
                User = role_Master.User,
                Admin = role_Master.Admin,
                IsActive = role_Master.IsActive,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            await _role_MasterRepository.AddRole_Master(result);
            return RedirectToAction("GetsAllRole_Masters", "Role_Master");
        }
        public IActionResult EditRole_Masters(Guid id)
        {
            var result = _role_MasterRepository.GetRole_MasterById(id);
            return View(result);
        }
        public async Task<IActionResult> EditRoleMasters(Role_Master role_Master)
        {
            var result = await _role_MasterRepository.UpdateRole_Master(role_Master);
            if (result != null)
            {
                TempData["SuccessMessage"] = "RoleMaster Edited Successfully!...";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Edit RoleMaster!...";
            }
            return RedirectToAction("GetsAllRole_Masters", "Role_Master");
        }
        public async Task<IActionResult> DeleteRole_Master(Guid id)
        {
            await _role_MasterRepository.DeleteRole_Master(id);
            return RedirectToAction("GetsAllRole_Masters", "Role_Master");
        }
    }
}
