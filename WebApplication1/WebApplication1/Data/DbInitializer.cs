using WebApplication1.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Data
{
    public class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new HotelContext(serviceProvider
                .GetRequiredService<DbContextOptions<HotelContext>>()))
            {
                context.Database.EnsureCreated();
                context.SaveChanges();
            }
        }
    }
}
