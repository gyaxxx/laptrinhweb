namespace WebApplication1.Models
{
    public class GuestRoom_Booking
    {
        public Guest guest { get; set; }
        public Room room { get; set; }
        public Reservations reservations { get; set; }
    }
}
