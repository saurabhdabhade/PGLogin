using System.ComponentModel.DataAnnotations;

namespace PGLogin.Models.DTO
{
    public class RegisterDTO
    {
        [MaxLength(30)]
        [Required]
        public string First_Name { get; set; }

        [MaxLength(30)]
        [Required]
        public string Last_Name { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
        [EmailAddress]
        [StringLength(30)]
        [Required]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 50 characters.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,50}$",
            ErrorMessage = "Password must contain at least one uppercase, one lowercase, one number, and one special character.")]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Confirm_Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 50 characters.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,50}$",
            ErrorMessage = "Password must contain at least one uppercase, one lowercase, one number, and one special character.")]
        public string Confirm_Password { get; set; }

        public string LastPassword1 { get; set; }
        public string LastPassword2 { get; set; }

        public DateTime EventDateTime { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; }
    }
}
