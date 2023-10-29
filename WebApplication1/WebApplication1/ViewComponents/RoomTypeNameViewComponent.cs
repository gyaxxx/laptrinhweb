using WebApplication1.Data;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using System.Collections.Generic;
using System.Linq;
namespace WebApplication1.ViewConponents
{
    public class RoomTypeNameViewComponent : ViewComponent
    {
        HotelContext database;
        List<Room> rooms;

        public RoomTypeNameViewComponent(HotelContext _context)
        {
            database = _context;
            rooms = database.Rooms.ToList();
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("RenderRoomTypeName", rooms);
        }

    }
}
