using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PGLogin.Models;
using PGLogin.Models.Data;
using PGLogin.Models.DTO;
using PGLogin.Models.Repository;
using PGLogin.Models.Repository.IRepository;

namespace PGLogin.Controllers
{
    public class CandidateController : Controller
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly MydBContext _mydBContext;
        private readonly IWebHostEnvironment _env;

        public CandidateController(ICandidateRepository candidateRepository, MydBContext mydBContext, IWebHostEnvironment env)
        {
            _candidateRepository = candidateRepository;
            _mydBContext = mydBContext;
            _env = env;
        }
        public async Task<IActionResult> GetsCandidates(int page = 1)
        {
            var folder = "images";
            string imagesPath = Path.Combine(_env.WebRootPath, folder);

            var imageFiles = Directory.GetFiles(imagesPath)
                                      .Select(file => folder + Path.GetFileName(file))
                                      .ToList();
            ViewBag.Images = imageFiles;
            int pageSize = 15;
            var candidates = await _mydBContext.candidates.OrderBy(r => r.CandidateId).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(_mydBContext.candidates.Count() / (double)pageSize);
            return View(candidates); // Pass list to view
        }

        public async Task<IActionResult> GetCandidates()
        {
            var candidates = await _candidateRepository.GetAllCandidates();

            // Ensure the repository actually returns a List<Candidate>
            // and NOT an IActionResult

            return View("GetsCandidates", candidates); // 👈 View name must match your Razor file
        }
        [Route("CreateCandidate")]
        public IActionResult CreateCandidate()
        {
            return View();
        }

        public async Task<IActionResult> Create(Candidate candidates)
        {
            var result = new CandidateDTO()
            {
                FullName = candidates.FullName,
                Phone = candidates.Phone,
                Email = candidates.Email,
                Address = candidates.Address,
                CreatedAt = candidates.CreatedAt,
            };
            if (candidates.PhotoFile != null && candidates.PhotoFile.Length > 0)
            {
                string folder = "images";
                string uploadsFolder = Path.Combine(_env.WebRootPath, folder);

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(candidates.PhotoFile.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await candidates.PhotoFile.CopyToAsync(stream);
                }

                // IMPORTANT: Assign to result, not rooms
                result.My_Photo = "/" + folder + "/" + fileName;
            }
            await _candidateRepository.AddCandidate(result);
            return RedirectToAction("GetsCandidates", "Candidate");
        }

        [HttpGet]
        public async Task<IActionResult> EditCandidate(Guid id)
        {
            var candidate = await _candidateRepository.GetCandidateById(id);

            if (candidate == null)
            {
                TempData["ErrorMessage"] = "Candidate not found!";
                return RedirectToAction("GetCandidates", "Candidate");
            }
            return View(candidate);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Candidate candidate)
        {
            if (!ModelState.IsValid)
            {
                return View(candidate);
            }
            var response = await _candidateRepository.UpdateCandidate(candidate);

            if (response != null)
            {
                TempData["SuccessMessage"] = "Candidate edited successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to edit Candidate!";
            }
            return RedirectToAction("GetCandidates", "Candidate");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCandidate(Guid id)
        {
            await _candidateRepository.DeleteCandidate(id);
            return RedirectToAction("GetCandidates", "Candidate");
        }
    }
}
