using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PGLogin.Models
{
    public class Booking
    {
        [Key]
        public Guid BookingId { get; set; } = Guid.NewGuid();

        [ForeignKey(nameof(BookingId))]
        [Required(ErrorMessage = "Tenant ID is required")]
        public Guid CandidateId { get; set; }
        public Candidate? Candidate { get; set; }

        [ForeignKey(nameof(CandidateId))]
        [Required(ErrorMessage = "Room ID is required")]
        public Guid RoomId { get; set; }
        public Room? Room { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format for start date")]
        [CustomValidation(typeof(Booking), nameof(ValidateStartDate))]
        public DateTime StartDate { get; set; } = DateTime.UtcNow.Date;

        [DataType(DataType.Date, ErrorMessage = "Invalid date format for end date")]
        [CustomValidation(typeof(Booking), nameof(ValidateEndDate))]
        public DateTime? EndDate { get; set; } = null;

        [Required(ErrorMessage = "Active status must be specified")]
        public bool Active { get; set; } = true;

        [Required(ErrorMessage = "Security deposit is required")]
        [Range(0, 100000, ErrorMessage = "Security deposit must be between ₹0 and ₹100,000")]
        public decimal SecurityDeposit { get; set; } = 0m;

        // ================= Seats Property =================

        [Range(1, 4, ErrorMessage = "Seats must be between 1 and 4")]
        [CustomValidation(typeof(Booking), nameof(ValidateSeats))]
        public int Seats { get; init; }

        // ================= Custom Validation Methods =================

        public static ValidationResult? ValidateStartDate(DateTime startDate, ValidationContext context)
        {
            if (startDate.Date > DateTime.UtcNow.Date.AddYears(1))
                return new ValidationResult("Start date cannot be more than 1 year in the future");

            return ValidationResult.Success;
        }

        public static ValidationResult? ValidateEndDate(DateTime? endDate, ValidationContext context)
        {
            if (endDate.HasValue)
            {
                var instance = (Booking)context.ObjectInstance;

                if (endDate.Value.Date < instance.StartDate.Date)
                    return new ValidationResult("End date cannot be earlier than start date");
            }

            return ValidationResult.Success;
        }

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
