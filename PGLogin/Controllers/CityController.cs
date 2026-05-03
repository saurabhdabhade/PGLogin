using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using PGLogin.Models;
using PGLogin.Models.Data;
using PGLogin.Models.DTO;
using PGLogin.Models.Repository.IRepository;

namespace PGLogin.Controllers
{
    public class CityController : Controller
    {
        private readonly ICityRepository _cityRepository;
        private readonly MydBContext _mydBContext;
        public CityController(ICityRepository cityRepository, MydBContext mydBContext)
        {
            _cityRepository = cityRepository;
            _mydBContext = mydBContext;
        }
        [HttpGet]
        public IActionResult GetAllCities()
        {
            var cities = _mydBContext.cities.Include(c => c.Areas).ToList();
            return View(cities);
        }
        public async Task<IActionResult> GetCities()
        {
            var result = _cityRepository.GetAllCities();
            return View("GetAllCities", result);
        }

        public IActionResult CreateCity()
        {
            return View();
        }

        // POST: Add City to DB
        [HttpPost]
        public async Task<IActionResult> CreateCity(City cityDto)
        {
            try
            {
                var createdCity = await _cityRepository.AddCity(cityDto);

                if (createdCity == null)
                {
                    TempData["Error"] = "City could not be created. Try again.";
                    return View(cityDto);
                }

                TempData["Success"] = "City added successfully!";
                return RedirectToAction("GetAllCities", "City");
            }
            catch (DbUpdateException ex)
            {
                var message = ex.InnerException?.Message;
                throw new Exception(message);
            }
            catch (Exception ex)
            {
                // Log ex if you have logging
                TempData["Error"] = "An unexpected error occurred.";
                return View(cityDto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditCity(Guid id)
        {
            var city = await _cityRepository.GetCityById(id);

            if (city == null)
            {
                TempData["ErrorMessage"] = "City not found!";
                return RedirectToAction("GetAllCities", "City");
            }
            return View(city);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(City city)
        {
            if (!ModelState.IsValid)
            {
                return View(city);
            }
            var response = await _cityRepository.UpdateCity(city);

            if (response != null)
            {
                TempData["SuccessMessage"] = "Room edited successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to edit room!";
            }
            return RedirectToAction("GetAllCities", "City");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCity(Guid cityId)
        {
            await _cityRepository.DeleteCity(cityId);
            return RedirectToAction("GetAllCities", "City");
        }
    }
}
