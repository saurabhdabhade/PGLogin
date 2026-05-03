using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PGLogin.Models;
using PGLogin.Models.Data;
using PGLogin.Models.DTO;
using PGLogin.Models.Repository;
using PGLogin.Models.Repository.IRepository;

namespace PGLogin.Controllers
{
    public class RoomController : Controller
    { 
        private readonly IRoomRepository _roomRepository;
        private readonly MydBContext _mydBContext;
        private readonly IWebHostEnvironment _env;

        public RoomController(IRoomRepository roomRepository, MydBContext mydBContext, IWebHostEnvironment env)
        {
            _roomRepository = roomRepository;
            _mydBContext = mydBContext;
            _env = env;
        }
        public async Task<IActionResult> GetsRooms(int page = 1)
        {
            var folder = "images";
            string imagesPath = Path.Combine(_env.WebRootPath, folder);

            var imageFiles = Directory.GetFiles(imagesPath)
                                      .Select(file => folder + Path.GetFileName(file))
                                      .ToList();
            ViewBag.Images = imageFiles;
            int pageSize = 15;
            var rooms = await _mydBContext.rooms.OrderBy(r => r.RoomNumber).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(_mydBContext.rooms.Count() / (double)pageSize);
            return View(rooms);
        }

        public async Task<IActionResult> GetRooms()
        {
            var result = _roomRepository.GetAllRooms();
            return View("GetsRooms", result);
        }

        public IActionResult CreateRoom()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Room rooms)
         {
            var result = new RoomDTO()
            {
                RoomNumber = rooms.RoomNumber,
                RentPerMonth = rooms.RentPerMonth,
                Status = rooms.Status,
                Notes = rooms.Notes,
                Address = rooms.Address, 
                Description = rooms.Description,
                Seats = rooms.Seats,
            };

            // IMAGE UPLOAD LOGIC
            if (rooms.ImageFile != null && rooms.ImageFile.Length > 0)
            {
                string folder = "images";
                string uploadsFolder = Path.Combine(_env.WebRootPath, folder);

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(rooms.ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await rooms.ImageFile.CopyToAsync(stream);
                }

                // IMPORTANT: Assign to result, not rooms
                result.ImagePath = "/" + folder + "/" + fileName;
            }

            await _roomRepository.AddRoom(result);

/*            TempData["SuccessMessage"] = "Room added successfully!";
*/            return RedirectToAction("GetsRooms", "Room");
        }


        [HttpGet]
        public async Task<IActionResult> EditRoom(Guid id)
        {
            var room = await _roomRepository.GetRoomById(id);

            if (room == null)
            {
                TempData["ErrorMessage"] = "Room not found!";
                return RedirectToAction("GetsRooms", "Room");
            }
            return View(room);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Room room)
        {
            if (room.ImageFile != null)
            {
                string folder = Path.Combine("images");
                string uploadsFolder = Path.Combine(_env.WebRootPath, folder);

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(room.ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await room.ImageFile.CopyToAsync(stream);
                }

                room.ImagePath = "/" + folder + "/" + fileName;
            }

            _mydBContext.Update(room);
            await _mydBContext.SaveChangesAsync();

            return RedirectToAction("GetsRooms", "Room");
        }

        // GET: Room/Delete/{id}  -> shows a confirmation view (optional)
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            var room = _mydBContext.rooms.Find(id);
            if (room == null) return NotFound();
            return View(room); // optional confirmation page
        }

        // POST: Room/Delete/{id} -> actually deletes
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var room = _mydBContext.rooms.Find(id);
            if (room == null) return NotFound();

            _mydBContext.rooms.Remove(room);
            _mydBContext.SaveChanges();
            /*            TempData["SuccessMessage"] = "Room deleted successfully.";
            */
            return RedirectToAction(nameof(GetsRooms));
        }

        public IActionResult Details(Guid id)
        {
            var room = _mydBContext.rooms.FirstOrDefault(x => x.RoomId == id);
            if (room == null)
                return NotFound();

            return View(room);
        }
        public async Task<IActionResult> GetDetails(Guid id)
        {
            var room = await _roomRepository.GetRoomById(id);

            if (room == null)
                return NotFound();

            return View("Details", room);
        }
    }
}