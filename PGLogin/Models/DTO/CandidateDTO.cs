using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PGLogin.Models.DTO
{
    public class CandidateDTO
    {
        [Required(ErrorMessage = "Full name is required")]
        public string FullName { get; set; } = "";

        [Phone(ErrorMessage = "Invalid phone number format")]
        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Phone number must be between 10 and 15 digits")]
        public string Phone { get; set; } = "";

        [EmailAddress(ErrorMessage = "Invalid email format")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
        [StringLength(30)]
        [Required]
        public string Email { get; set; } = "";
        public string Address { get; set; } = "";

        [Display(Name = "Candidate Photo")]
        public string? My_Photo { get; set; }  // Stores image file path (saved in DB)

        [NotMapped]
        [Display(Name = "Upload Photo")]
        public IFormFile? PhotoFile { get; set; } // Used only for form upload
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
