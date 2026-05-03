using Microsoft.AspNetCore.Mvc;
using PGLogin.Models;
using PGLogin.Models.Data;
using PGLogin.Models.DTO;
using PGLogin.Models.Repository;
using PGLogin.Models.Repository.IRepository;

namespace PGLogin.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly MydBContext _dbContext;    
        public BookingController(IBookingRepository bookingRepository, MydBContext dbContext)
        {
            _bookingRepository = bookingRepository;
            _dbContext = dbContext;
        }
        public IActionResult GetsBookings()
        {
            var result = _dbContext.bookings.ToList();
            return View(result);
        }
        public async Task<IActionResult> GetBookings()
        {
            var result = _bookingRepository.GetAllBookings();
            return View("GetsBookings", result);
        }

        public IActionResult CreateBooking()
        {
            return View();
        }
        public async Task<IActionResult> Create(Booking books)
        {
            var result = new BookingDTO()
            {
                CandidateId = books.CandidateId,
                RoomId = books.RoomId,
                StartDate = books.StartDate,
                EndDate = books.EndDate,
                Active = books.Active,
                SecurityDeposit = books.SecurityDeposit,
                Seats = books.Seats,
            };
            ViewBag.RoomId = books.RoomId;  
            await _bookingRepository.AddBooking(result);
            string subject = "Congratulations!";
            string body = "Congratulations, Your Have Booked The Room Successfully!...";
            EmailSender sender = new EmailSender();
            Register register   = new Register();
            sender.SendEmail(register.Email, subject, body);
            return RedirectToAction("GetsRooms", "Room");
        }

        [HttpGet] 
        public async Task<IActionResult> EditRoom(Guid id)
        {
            var booking = await _bookingRepository.GetBookingById(id);

            if (booking == null)
            {
                TempData["ErrorMessage"] = "Booking not found!";
                return RedirectToAction("GetsBookings", "Booking");
            }
            return View(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Booking booking)
        {
            if (!ModelState.IsValid)
            {
                return View(booking);
            }
            var response = await _bookingRepository.UpdateBooking(booking);

            if (response != null)
            {
                TempData["SuccessMessage"] = "Booking edited successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to edit Booking!";
            }
            return RedirectToAction("GetsBookings", "Booking");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBooking(Guid id)
        {
            await _bookingRepository.DeleteBooking(id);
            return RedirectToAction("GetsBookings", "Booking");
        }
    }
}
