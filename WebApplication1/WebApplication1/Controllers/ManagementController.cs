using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ManagementController : Controller
    {

        private readonly HotelContext database;

        public ManagementController(HotelContext context)
        {
            this.database = context;
        }
        
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("Username:");
            return RedirectToAction("Login", "Hotel");
        }

        public IActionResult Staff()
        {
            var username = HttpContext.Session.GetString("Username");
            ViewBag.Username = username;

            ViewBag.Role = HttpContext.Session.GetString("Role");

            var allRes = database.Reservations.ToList();

            foreach(var res in allRes)
            {
                Reservations temp = new Reservations();
                int roomId = temp.RoomID;

                bool hasReservations = allRes.Any(r => r.RoomID == roomId);

                if(hasReservations)
                {
                    Room room = database.Rooms.FirstOrDefault(r => r.Id == roomId);
                    if(room != null)
                    {
                        room.IsActive = false;
                    }
                }
            }

            var totalRoom = database.Rooms.Count();
            var inhouseGuest = database.Guests.Count();
            var resCount = database.Reservations.Count();
            float income = 0;
            foreach(var temp in database.Reservations)
            {
                income += temp.Income;     
            }
            var countActiveRooms = database.Rooms
                .Count(r => r.IsActive == true);

            ViewBag.totalRoom = totalRoom;
            ViewBag.RoomActiveCount = countActiveRooms;

            ViewBag.guestCount = inhouseGuest;
            ViewBag.resCount = resCount;

            ViewBag.Income = income;

            return View();
        }
        public IActionResult StaffList(int pg = 1)
        {
            List<Staff> staff = database.Staffs.ToList();
            const int pageSize = 8;
            if (pg < 1) pg = 1;
            int recordCount = staff.Count();
            var pager = new Pager(recordCount, pg, pageSize);
            int recordSkip = (pg - 1) * pageSize;
            var data = staff.Skip(recordSkip).Take(pager.PageSize).ToList();
            ViewBag.Pager = pager;

            return View("StaffList", data);
        }

        public IActionResult Room(int pg = 1)
        {
            List<Room> room = database.Rooms.ToList();
            const int pageSize = 8;
            if (pg < 1) pg = 1;
            int recordCount = room.Count();
            var pager = new Pager(recordCount, pg, pageSize);
            int recordSkip = (pg - 1) * pageSize;
            var data = room.Skip(recordSkip).Take(pager.PageSize).ToList();
            ViewBag.Pager = pager;

            return View("Room", data);
        }

        public IActionResult RoomByName(string name, int pg = 1)
        {

            var data = database.Rooms
                .Where(r => r.RoomTypeName == name)
                .ToList();
            
            return PartialView("_RoomData", data);
        }

        public IActionResult Reservation(int pg = 1)
        {
            List<Reservations> reservation = database.Reservations.ToList();
            const int pageSize = 8;
            if (pg < 1) pg = 1;
            int recordCount = reservation.Count();
            var pager = new Pager(recordCount, pg, pageSize);
            int recordSkip = (pg - 1) * pageSize;
            var data = reservation.Skip(recordSkip).Take(pager.PageSize).ToList();
            ViewBag.Pager = pager;

            ViewBag.recordCount = recordCount;

            return View("Reservation", data);
        }

        public IActionResult GuestList(int pg = 1)
        {
            List<Guest> guest = database.Guests.ToList();
            const int pageSize = 8;
            if (pg < 1) pg = 1;
            int recordCount = guest.Count();
            var pager = new Pager(recordCount, pg, pageSize);
            int recordSkip = (pg - 1) * pageSize;
            var data = guest.Skip(recordSkip).Take(pager.PageSize).ToList();
            ViewBag.Pager = pager;

            ViewBag.recordCount = recordCount;

            return View("GuestList", data);
        }

        public IActionResult Delete(int? id)
        {
            if(id == null || database.Reservations == null)
            {
                return NotFound();
            }

            var reservation = database.Reservations
                .FirstOrDefault(m => m.ID == id);

            if(reservation == null)
            {
                return NotFound();
            }
            return View(reservation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id) 
        {
            if(database.Reservations == null)
            {
                return Problem("Entity set 'Reservations' is null");
            }

            var res = database.Reservations.Find(id);
            int roomID = id;

            var room = database.Rooms.Find(roomID);
            var guest = database.Guests.Find(res.GuestID);
            if (room != null && database.Reservations != null && guest != null)
            {
                room.IsActive = true;
                database.Guests.Remove(guest);
                database.Reservations.Remove(res);
            }
            database.SaveChanges();



            return RedirectToAction("Reservation");
        }

        public IActionResult Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var guest = database.Guests.Find(id);

            if (guest == null)
            {
                return NotFound();
            }

            return View(guest);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Guest guest)
        {
            if(id != guest.ID)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    database.Update(guest);
                    database.SaveChanges();
                }
                catch
                {
                    if(!GuestExist(guest.ID))
                    {
                        return NotFound();
                    } else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(GuestList));
            }
            return View(guest);
        }

        private bool GuestExist(int id)
        {
            return database.Guests.Any(e => e.ID == id);   
        }
    }
}
