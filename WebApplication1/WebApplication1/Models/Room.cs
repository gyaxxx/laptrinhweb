using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Room
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage ="Must choose type")]
        public string RoomNumber { get; set; } = null!;
        public int Floor { get; set; }
        public string RoomTypeName { get; set; } = null!;
        public string? Service { get; set; }
        public float Price { get; set; }
        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }
}
