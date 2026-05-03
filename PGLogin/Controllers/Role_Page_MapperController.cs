using Microsoft.AspNetCore.Mvc;
using PGLogin.Models;
using PGLogin.Models.Data;
using PGLogin.Models.DTO;
using PGLogin.Models.Repository.IRepository;

namespace PGLogin.Controllers
{
    public class Role_Page_MapperController : Controller
    {
        private readonly IRole_Page_Mapper_Repository _Page_Mapper_Repository;
        private readonly MydBContext _mydBContext;

        public Role_Page_MapperController(IRole_Page_Mapper_Repository Page_Mapper_Repository, MydBContext mydBContext)
        {
            _Page_Mapper_Repository = Page_Mapper_Repository;
            _mydBContext = mydBContext;
        }
        public IActionResult GetAllRole_Page_Mappers(Guid id)
        {
            var result = _Page_Mapper_Repository.GetRole_Page_MapperId(id);
            return View(result);
        }
        public async Task<IActionResult> AllRole_Page_Mappers()
        {
            var result = _Page_Mapper_Repository.GetAllRole_Page_Mappers();
            return View("Role_Page_Mapper", result);
        }
        public IActionResult CreateRole_Page_Mapper(Guid id)
        {
            return View();
        }
        public async Task<IActionResult> CreateRolePageMapper(Role_Page_Mapper role_Page_Mapper)
        {
            var result = new Role_Page_MapperDTO
            {
                Role_Id = role_Page_Mapper.Role_Id,
                Page_Id = role_Page_Mapper.Page_Id,
                IsActive = role_Page_Mapper.IsActive,
            };
            await _Page_Mapper_Repository.AddRole_Page_Mapper(result);
            return RedirectToAction("GetAllRole_Page_Mappers", "Role_Page_Mapper");
        }
        public IActionResult EditRole_Page_Mappers(Guid id)
        {
            var result = _Page_Mapper_Repository.GetRole_Page_MapperId(id);
            return View(result);
        }
        public async Task<IActionResult> Role_Page_Mapper_Edit(Role_Page_Mapper role_Page_Mapper)
        {
            var result = _Page_Mapper_Repository.UpdateRole_Page_Mapper(role_Page_Mapper);
            if (result != null)
            {
                TempData["SuccessMessage"] = "Role Page Is Edited Successfully!...";
            }
            else
            {
                TempData["ErrorMessage"] = " Failed To Edit Role Page!...";
            }
            return RedirectToAction("GetAllRole_Page_Mappers", "Role_Page_Mapper");
        }
        public async Task<IActionResult> DeleteRole_Page_Mapper(Guid id)
        {
            await _Page_Mapper_Repository.DeleteRole_Page_Mapper(id);
            return RedirectToAction("GetAllRole_Page_Mappers", "Role_Page_Mapper");
        }
    }
}
