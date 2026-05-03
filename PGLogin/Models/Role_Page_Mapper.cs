using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PGLogin.Models
{
    public class Role_Page_Mapper
    {
        [Key]
        [Required(ErrorMessage = "Role_Page_ID is required.")]
        public Guid Role_Page_ID { get; set; }

        // Foreign Key → Role_Master
        [Required(ErrorMessage = "Role_Id is required.")]
        public Guid Role_Id { get; set; }

        [ForeignKey(nameof(Role_Id))]
        public Role_Master? Role_Master { get; set; }

        // Foreign Key → Page_Master
        [Required(ErrorMessage = "Page_Id is required.")]
        public Guid Page_Id { get; set; }

        [ForeignKey(nameof(Page_Id))]
        public Page_Master? Page_Master { get; set; }

        // Active Flag
        [Required(ErrorMessage = "IsActive field is required.")]
        public bool IsActive { get; set; } = true;
    }
}
