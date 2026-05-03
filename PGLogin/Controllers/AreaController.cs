using Microsoft.AspNetCore.Mvc;
using PGLogin.Models;
using PGLogin.Models.Data;
using PGLogin.Models.DTO;
using PGLogin.Models.Repository.IRepository;

namespace PGLogin.Controllers
{
    public class AreaController : Controller
    {
        private readonly IAreaRepository _areaRepository;
        private readonly MydBContext _mydBContext;
        public AreaController(IAreaRepository areaRepository, MydBContext mydBContext)
        {
            _areaRepository = areaRepository;
            _mydBContext = mydBContext;
        }
        public IActionResult GetsAreas()
        {
            var result = _mydBContext.areas.ToList();
            return View(result);
        }
        public async Task<IActionResult> Gets_Areas()
        {
            var result = await _areaRepository.GetAllAreas();
            return View("GetsAreas", result);
        }
        public IActionResult Create_Area()
        {
            return View();
        }
        public async Task<IActionResult> CreateArea(Area area)
        {
            var result = new AreaDTO()
            {
                AreaName = area.AreaName,
                City_Id = area.City_Id,
            };
            ViewBag.AreaName = area.AreaName;   
            await _areaRepository.AddArea(result);
            return RedirectToAction("GetsAreas", "Area");
        }

        public IActionResult Edit_Area(Guid id)
        {
            var result = _areaRepository.GetAreaById(id);
            return View(result);
        }
        public async Task<IActionResult> Area_Edit(Area area)
        {
            var result = new AreaDTO()
            {
                AreaName = area.AreaName,
                City_Id = area.City_Id,
            };
            await _areaRepository.UpdateArea(area);
            if (result == null)
            {
                TempData["SuccessMessage"] = "Area Edited Successfully!...";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Edit Area!...";
            }
            return RedirectToAction("GetsAreas", "Area");
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            await _areaRepository.DeleteArea(id);
            return RedirectToAction("GetsAreas", "Area");
        }
    }
}
