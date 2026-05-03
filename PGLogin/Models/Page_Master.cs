using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PGLogin.Models
{
    public class Page_Master
    {
        [Key]
        [Required(ErrorMessage = "Page Id is required.")]
        public Guid Page_Id { get; set; } = Guid.NewGuid();


        [Required(ErrorMessage = "Page name is required.")]
        [StringLength(150, MinimumLength = 2,
            ErrorMessage = "Page name must be between 2 and 150 characters.")]
        [RegularExpression(@"^[A-Za-z0-9\s\-_]+$",
            ErrorMessage = "Page name can only contain letters, numbers, spaces, hyphens and underscores.")]
        public string Page_Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "IsActive flag is required.")]
        public bool IsActive { get; set; } = true;
    }
}
