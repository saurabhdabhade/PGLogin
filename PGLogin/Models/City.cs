using System.ComponentModel.DataAnnotations;

namespace PGLogin.Models
{
    public class City
    {
        [Key]
        public Guid City_Id { get; set; }

        [Required(ErrorMessage = "City name is required")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "City name must be between 2 and 100 characters")]
        [RegularExpression(@"^[A-Za-z\s]+$",
            ErrorMessage = "City name can contain only letters and spaces")]
        public string? City_Name { get; set; }

        [Required(ErrorMessage = "Area name is required.")]
        /*[StringLength(120, MinimumLength = 2,
            ErrorMessage = "Area name must be between 2 and 120 characters.")]*/
        [RegularExpression(@"^[A-Za-z0-9\s\-,]+$",
            ErrorMessage = "Area name can contain letters, numbers, spaces, commas, and hyphens only.")]
        public List<string>? AreaName { get; set; } = new List<string>();

        // Navigation property — NO validation attributes
        public List<Area>? Areas { get; set; } = new List<Area>();
    }
}
