using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PGLogin.Models.DTO
{
    public class UserDTO
    {
        [Required(ErrorMessage = "User name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "User name must be between 2 and 50 characters.")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "User name can contain only alphabets and spaces.")]
        public string User_Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string User_Email { get; set; } = string.Empty;

        // -------------------------------
        // Foreign Key → Role_Master table
        // -------------------------------
        [Required(ErrorMessage = "Role_Id is required.")]
        [ForeignKey(nameof(Role_Master))]   // 👈 Correct FK mapping
        public Guid Role_Id { get; set; }

        public Role_Master? Role_Master { get; set; }

        // ---------------------------------------------------------
        //           Additional Model-Level Validation
        // ---------------------------------------------------------
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Email must be corporate/business logic example (optional)
            if (User_Email.EndsWith("@test.com", StringComparison.OrdinalIgnoreCase))
            {
                yield return new ValidationResult(
                    "Test domain emails are not allowed.",
                    new[] { nameof(User_Email) }
                );
            }

            // Ensure Guid validity
            if (Role_Id == Guid.Empty)
            {
                yield return new ValidationResult(
                    "Role_Id cannot be an empty GUID.",
                    new[] { nameof(Role_Id) }
                );
            }

            yield break;
        }
    }
}
