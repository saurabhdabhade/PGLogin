using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PGLogin.Models.DTO
{
    public class BookingDTO
    {
        [ForeignKey("Candidate")]
        [Required(ErrorMessage = "Candidate ID is required")]
        public Guid CandidateId { get; set; }

        [ForeignKey("Room")]
        [Required(ErrorMessage = "Room ID is required")]
        public Guid RoomId { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow.Date;

        public DateTime? EndDate { get; set; } = null;

        [Required(ErrorMessage = "Active status must be specified")]
        public bool Active { get; set; } = true;

        [Required(ErrorMessage = "Security deposit is required")]
        [Range(0, 100000, ErrorMessage = "Security deposit must be between ₹0 and ₹100,000")]
        public decimal SecurityDeposit { get; set; } = 0m;

        [Range(1, 4, ErrorMessage = "Seats must be between 1 and 4")]
        [CustomValidation(typeof(Booking), nameof(ValidateSeats))]
        public int Seats { get; init; }

        // Custom validation for Seats (optional but recommended)
        public static ValidationResult? ValidateSeats(int seats, ValidationContext context)
        {
            // Example rule: No booking should have more than 4 seats if Active booking exists
            if (seats > 4)
            {
                return new ValidationResult("A single booking cannot have more than 4 seats");
            }

            return ValidationResult.Success;
        }
    }
}
