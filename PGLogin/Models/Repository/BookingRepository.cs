using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using PGLogin.Models.Data;
using PGLogin.Models.DTO;
using PGLogin.Models.Repository.IRepository;

namespace PGLogin.Models.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly MydBContext _Context;
        public BookingRepository(MydBContext context)
        {
            _Context = context;
        }
        public async Task<Booking> AddBooking(BookingDTO booking)
        {
            var bookings = new Booking
            {
                CandidateId = booking.CandidateId,
                RoomId = booking.RoomId,  
                StartDate = booking.StartDate,    
                EndDate = booking.EndDate,
                Active = booking.Active,
                SecurityDeposit = booking.SecurityDeposit,
                Seats = booking.Seats,
            };

            await _Context.bookings.AddRangeAsync(bookings);
            await _Context.SaveChangesAsync();

            return await Task.FromResult(bookings);
        }

        public async Task<Booking> DeleteBooking(Guid bookingId)
        {
            var result = await _Context.bookings.FirstOrDefaultAsync(x => x.BookingId == bookingId);
            if (result != null)
            {
                _Context.bookings.Remove(result);
                await _Context.SaveChangesAsync();
            }
            return result;
        }

        public async Task<Booking> GetBookingById(Guid bookingId)
        {
            try
            {
                var result = await _Context.bookings.FirstOrDefaultAsync(x => x.BookingId == bookingId);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<Booking>> GetAllBookings()
        {
            return await _Context.bookings.ToListAsync();
        }

        public async Task<Booking> UpdateBooking(Booking booking)
        {
            var result = await _Context.bookings.FirstOrDefaultAsync(x => x.BookingId == booking.BookingId);
            if(result != null)
            {
                result.BookingId = booking.BookingId; 
                result.CandidateId = booking.CandidateId;
                result.RoomId = booking.RoomId;
                result.StartDate = booking.StartDate;
                result.EndDate = booking.EndDate;
                result.Active = booking.Active;
                result.SecurityDeposit = booking.SecurityDeposit;
                await _Context.SaveChangesAsync();
            }
            return result;
        }
    }
}
