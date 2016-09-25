using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWedding.Data;
using MyWedding.Models;


namespace MyWedding.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Guests.ToList());
        }

        [HttpPost]
        public IActionResult AddGuest([FromForm] string code, string name)
        {
             var guest = new Guest();
            guest.Code = code;
            guest.Name = name;
            _context.Guests.Add(guest);
            _context.SaveChanges();

            return View("Index", _context.Guests.ToList());
        }
    }
}
