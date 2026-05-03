using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PGLogin.Models;
using PGLogin.Models.Data;
using PGLogin.Models.DTO;
using PGLogin.Models.Repository;
using PGLogin.Models.Repository.IRepository;

namespace PGLogin.Controllers
{
    public class PGController : Controller
    {
        private readonly IPGRepository _pGRepository;
        private readonly MydBContext _mydBContext;
        private readonly IWebHostEnvironment _env;

        public PGController(IPGRepository pGRepository, MydBContext mydBContext, IWebHostEnvironment env)
        {
            _pGRepository = pGRepository;
            _mydBContext = mydBContext;
            _env = env; 
        }
        public async Task<IActionResult> GetsPGs(int page = 1)
        {
            var folder = "images";
            string imagesPath = Path.Combine(_env.WebRootPath, folder);

            var imageFiles = Directory.GetFiles(imagesPath)
                                      .Select(file => folder + Path.GetFileName(file))
                                      .ToList();
            ViewBag.Images = imageFiles;
            int pageSize = 15;
            var pgs = await _mydBContext.pGs.OrderBy(r => r.PG_Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(_mydBContext.pGs.Count() / (double)pageSize); 
            return View(pgs);
        }
        public async Task<IActionResult> Gets_PGs()
        {
            var Result = await _pGRepository.GetAllPGs();
            return View("GetsPGs", Result);
        }

        public IActionResult Create_PG()
        {
            return View();
        }
        public async Task<IActionResult> CreatePG(PG pg)
        {
            var Result = new PGDTO()
            {
                PG_Name = pg.PG_Name,
                Price = pg.Price,
                SharingType = pg.SharingType,
                AreaId = pg.AreaId,
            };
            ViewBag.Price = pg.Price;
            if (pg.ImageFile != null && pg.ImageFile.Length > 0)
            {
                string folder = "images";
                string uploadsFolder = Path.Combine(_env.WebRootPath, folder);

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(pg.ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await pg.ImageFile.CopyToAsync(stream);
                }

                // IMPORTANT: Assign to result, not rooms
                Result.ImagePath = "/" + folder + "/" + fileName;
            }
            await _pGRepository.AddPG(Result);
            TempData["SuccessMessage"] = "PG Added Successfully!...";
            return RedirectToAction("GetsPGs", "PG");
        }
        public IActionResult Edit_PG(Guid id)
        {
            var result = _pGRepository.GetPGById(id);
            return View(result);
        }
        public async Task<IActionResult> EditPG(PG pg)
        {
            var result = await _pGRepository.UpdatePG(pg);
            if(result != null)
            {
                TempData["SuccessMessage"] = "PG Edited Successfully!...";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to Add PG!...";
            }
            return RedirectToAction("GetsPGs", "PG");
        }
        public async Task<IActionResult> Delete_PG(Guid id)
        {
            await _pGRepository.DeletePG(id);
            return RedirectToAction("GetsPGs", "PG");
        }

        public async Task<IActionResult> Search(PGDTO filter)
        {
            var results = await _pGRepository.Search_PG(filter);

            ViewBag.Cities = _mydBContext.cities.ToList();
            ViewBag.Areas = _mydBContext.areas.ToList();

            return View();
        }

    }
}
