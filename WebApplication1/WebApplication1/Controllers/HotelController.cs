using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HotelController : Controller
    {
        private readonly HotelContext database;
        public float totalMoney;

        public HotelController(HotelContext context)
        {
            this.database = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Staff inputData)
        {
            var user = database.Staffs.FirstOrDefault(s =>
                s.Username == inputData.Username && s.Password == inputData.Password);

            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);

                if (user.Role == "Staff" || user.Role == "Manager")
                {
                    return RedirectToAction("Staff", "Management");
                }
            }

            // If authentication fails, return to the login view
            return View();
        }

        public IActionResult Book() {
            return View();
        }

        [HttpGet]
        public IActionResult BookConfirmed() 
        {
            return View();
        }

        [HttpPost]
        public IActionResult BookConfirmed(GuestRoom_Booking data)
        {
            if(ModelState.IsValid)
            {
                if (data.reservations.CheckInDate >= data.reservations.CheckOutDate)
                {
                    ModelState.AddModelError("reservations.CheckInDate", "Check-in date must be earlier than the check-out date.");
                    return View("Book");
                }

                if(!database.Rooms.Any(r=>r.IsActive && r.RoomNumber == data.room.RoomNumber))
                {
                    ModelState.AddModelError("RoomNumber","Chosen room is booked");
                    return View("Book");
                }

                Guest newGuest = new()
                {
                    FirstName = data.guest.FirstName,
                    LastName = data.guest.LastName,
                    Email = data.guest.Email,
                    PhoneNumber = data.guest.PhoneNumber,
                };

                Room room = new()
                {
                    RoomNumber = data.room.RoomNumber,
                    Floor = data.room.Floor,
                    RoomTypeName = data.room.RoomTypeName,
                    IsActive = true,
                };
                   database.Guests.Add(newGuest);
                   database.SaveChanges();

                float rate = 1f; // Default rate

                if (data.reservations.CheckInDate.Hour >= 0 && data.reservations.CheckInDate.Hour < 9)
                {
                    rate = 0.5f;
                }
                else if (data.reservations.CheckInDate.Hour >= 9 && data.reservations.CheckInDate.Hour < 17)
                {
                    rate = 0.3f;
                }
                else if (data.reservations.CheckInDate.Hour >= 17 && data.reservations.CheckInDate.Hour < 24)
                {
                    rate = 1.0f;
                }

                float roomRate = 0.0f;

                if (data.room.RoomTypeName == "Superior")
                {
                    roomRate = 1000000;
                }
                else if (data.room.RoomTypeName == "Deluxe")
                {
                    roomRate = 1200000;
                }
                else if (data.room.RoomTypeName == "Senior Deluxe")
                {
                    roomRate = 1400000;
                }
                else if (data.room.RoomTypeName == "Family Room")
                {
                    roomRate = 1600000;
                }

                totalMoney = (float)((data.reservations.CheckOutDate.Day - data.reservations.CheckInDate.Day) * roomRate * rate);

                int newGuestid = newGuest.ID;
                int roomID = database.Rooms
                     .Where(r => r.RoomNumber == data.room.RoomNumber && r.Floor == data.room.Floor && r.RoomTypeName == data.room.RoomTypeName)
                     .Select(r => r.Id)
                     .FirstOrDefault();

                Reservations reservations = new()
                {
                    CreatedDate = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    GuestID = newGuestid,
                    RoomID = roomID,
                    CheckInDate = data.reservations.CheckInDate,
                    CheckOutDate = data.reservations.CheckOutDate,
                    Income = totalMoney,
                    Status = 1,
                };

                    database.Reservations.Add(reservations);
                    database.SaveChanges();
                TempData["successMessage"] = "Employee created successfully!";

                var reservedRoomIds = database.Reservations
                    .Select(res => res.RoomID)
                    .ToList();

                var roomsToDeactivate = database.Rooms.Where(r => reservedRoomIds.Contains(r.Id));

                foreach (var temp in roomsToDeactivate)
                {
                    temp.IsActive = false;
                }

                database.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();   
        }
    
    }
}