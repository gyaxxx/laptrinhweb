namespace WebApplication1.Models
{
    public class Invoice
    {
        public int ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string ResID { get; set; }
        public float Price { get; set; }
        public int Status { get; set; }
    }
}
