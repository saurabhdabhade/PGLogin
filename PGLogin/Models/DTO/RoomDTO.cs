using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PGLogin.Models.DTO
{
    public class RoomDTO
    {
        [Required(ErrorMessage = "Room number is required")]
        [StringLength(5, MinimumLength = 1, ErrorMessage = "Room number must be between 1 and 5 characters")]
        [RegularExpression(@"^[A-Za-z0-9\\-]+$", ErrorMessage = "Room number can only contain letters, numbers, and hyphens")]
        public string? RoomNumber { get; set; }

        [Required(ErrorMessage = "Rent per month is required")]
        [Range(1000, 10000, ErrorMessage = "Rent per month must be between ₹1,000 and ₹10,000")]
        public decimal? RentPerMonth { get; set; }

        [Required(ErrorMessage = "Room status is required")]
        [EnumDataType(typeof(RoomStatus), ErrorMessage = "Invalid room status value")]
        public RoomStatus Status { get; set; }

        [StringLength(100, ErrorMessage = "Notes cannot exceed 100 characters")]
        public string? Notes { get; set; }

        [Required(ErrorMessage = "Number of seats is required")]
        [Range(1, 4, ErrorMessage = "Seats must be between 1 and 5")]
        [CustomValidation(typeof(Room), nameof(ValidateSeats))]
        public int Seats { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 200 characters.")]
        [RegularExpression(@"^(?!\s*$)(?!\d+$).+",
    ErrorMessage = "Address cannot contain only numbers or be empty.")]
        public string? Address { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; }

        // ✅ NEW: Image properties
        [Display(Name = "Room Image")]
        public string? ImagePath { get; set; } // stores the image file path in DB

        [NotMapped]
        [Display(Name = "Upload Image")]
        public IFormFile? ImageFile { get; set; } // used for uploading from form

        [Required(ErrorMessage = "PG Id is required.")]
        [RegularExpression(@"^(\{)?[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\})?$",
            ErrorMessage = "Invalid PG Id format.")]
        [ForeignKey("PG")]
        public Guid PG_Id { get; set; }
        public PG? PG { get; set; } // Navigation Property

        // Custom validation for Seats (optional but recommended)
        public static ValidationResult? ValidateSeats(int seats, ValidationContext context)
        {
            // Example rule: No booking should have more than 4 seats if Active booking exists
            if (seats > 4)
            {
                return new ValidationResult("A single Room cannot have more than 4 seats");
            }

            return ValidationResult.Success;
        }
    }
}
