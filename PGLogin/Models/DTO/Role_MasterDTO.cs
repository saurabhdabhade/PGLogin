using System.ComponentModel.DataAnnotations;

namespace PGLogin.Models.DTO
{
    public class Role_MasterDTO
    {
        [Required(ErrorMessage = "User field is required.")]
        [StringLength(50, ErrorMessage = "User cannot exceed 50 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9_ ]+$", ErrorMessage = "User can contain only letters, numbers, spaces, and underscores.")]
        public string? User { get; set; }


        [Required(ErrorMessage = "Admin field is required.")]
        [StringLength(50, ErrorMessage = "Admin cannot exceed 50 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9_ ]+$", ErrorMessage = "Admin can contain only letters, numbers, spaces, and underscores.")]
        public string? Admin { get; set; }

        [Required(ErrorMessage = "IsActive flag must be specified.")]
        public bool IsActive { get; set; } = true;

        // Audit fields
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

    }
}
