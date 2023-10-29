using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Reservations
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int GuestID { get; set; }
        public int RoomID { get; set; }

        [Required(ErrorMessage = "Check-in date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime CheckInDate { get; set; }

        [Required(ErrorMessage = "Check-out date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime CheckOutDate { get; set; }
        public float Income { get; set; }
        public int Status { get; set; }
    }
}
