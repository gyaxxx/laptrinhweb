using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class HotelContext :DbContext
    {
        public HotelContext()
        {
        }

        public HotelContext(DbContextOptions<HotelContext> options) : base(options) { }
        public virtual DbSet<Guest> Guests { get; set; }
        public virtual DbSet<Reservations> Reservations { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Staff> Staffs { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guest>().ToTable(nameof(Guest)).HasKey(g => g.ID);
            modelBuilder.Entity<Reservations>().ToTable(nameof(Reservations)).HasKey(r => r.ID);
            modelBuilder.Entity<Room>().ToTable(nameof(Room)).HasKey(r => r.Id);
            modelBuilder.Entity<Staff>().ToTable(nameof(Staff)).HasKey(s => s.Id);
            modelBuilder.Entity<Invoice>().ToTable(nameof(Invoice)).HasKey(rt => rt.ID);
        }
    }
}
