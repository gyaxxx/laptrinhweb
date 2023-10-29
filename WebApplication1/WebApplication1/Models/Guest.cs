using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Guest
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required(ErrorMessage = "Firstname is required.")]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Lastname is required.")]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required(ErrorMessage ="Email is required.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
    }
}
