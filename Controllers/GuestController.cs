using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWedding.Data;
using MyWedding.Models;

namespace MyWedding.Controllers
{
    public class GuestController : Controller
    {
        private readonly ApplicationDbContext _context;
        public GuestController(ApplicationDbContext context) 
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Index([FromForm] string code)
        {
            var guest = _context.Guests.FirstOrDefault(x => x.Code == code);

            if (guest == null)
            {
                return NotFound();
            }

            return View(guest);
        }

        [HttpPost]
        public IActionResult SaveResponse([FromForm] int id, bool isAttending, EMealType mealType, string comments)
        {
            var guest = _context.Guests.FirstOrDefault(x => x.Id == id);
            
            guest.IsAttending = isAttending;
            guest.MealType = mealType;
            guest.Comments = comments;
            guest.HasResponded = true;

             _context.Guests.Update(guest);
            _context.SaveChanges();

            return View();
        }
    }
}
