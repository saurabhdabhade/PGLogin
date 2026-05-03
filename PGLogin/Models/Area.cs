using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PGLogin.Models
{
    public class Area
    {
        [Key]
        public Guid AreaId { get; set; }

        [Required(ErrorMessage = "Area name is required.")]
        [StringLength(120, MinimumLength = 2,
            ErrorMessage = "Area name must be between 2 and 120 characters.")]
        /*[RegularExpression(@"^[A-Za-z0-9\s\-,]+$",
            ErrorMessage = "Area name can contain letters, numbers, spaces, commas, and hyphens only.")]*/
        public string AreaName { get; set; } = string.Empty;

       [ForeignKey("City")]
        public Guid City_Id { get; set; }
        // Navigation property
        public City? City { get; set; }
        public ICollection<PG>? PGs { get; set; }

    }
}
