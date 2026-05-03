using PGLogin.Models.DTO;

namespace PGLogin.Models.Repository.IRepository
{
    public interface IBookingRepository
    {
        Task<Booking> GetBookingById(Guid bookingId);
        Task<IEnumerable<Booking>> GetAllBookings();
        Task<Booking> AddBooking(BookingDTO booking);
        Task<Booking> UpdateBooking(Booking booking);
        Task<Booking> DeleteBooking(Guid bookingId);
    }
}
