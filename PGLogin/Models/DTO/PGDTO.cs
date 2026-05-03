using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PGLogin.Models.DTO
{
    public class PGDTO
    {
        [Required(ErrorMessage = "PG name is required.")]
        [StringLength(150, MinimumLength = 3,
            ErrorMessage = "PG name must be between 3 and 150 characters.")]
        [RegularExpression(@"^[A-Za-z0-9\s\-,.&()]+$",
            ErrorMessage = "PG name can contain only letters, numbers, spaces, commas, hyphens, dots, ampersands, and parentheses.")]
        public string? PG_Name { get; set; }

        [Required(ErrorMessage = "Area Id is required.")]
        [ForeignKey("Area")]
        public Guid? AreaId { get; set; }

        // Navigation property — no validation attributes
        public Area? Area { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(500, 50000, ErrorMessage = "Price must be between ₹500 and ₹50,000.")]
        public decimal? Price { get; set; }        // Monthly Rent
         
        [Required(ErrorMessage = "Sharing type is required.")]
        [Range(1, 4, ErrorMessage = "Sharing type must be 1, 2, 3, or 4.")]
        public int? SharingType { get; set; }      // 1, 2, 3, 4 sharing

        [Display(Name = "PG Image")]
        public string? ImagePath { get; set; } // stores the image file path in DB

        [NotMapped]
        [Display(Name = "Upload Image")]
        public IFormFile? ImageFile { get; set; } // used for uploading from form
    }
}
