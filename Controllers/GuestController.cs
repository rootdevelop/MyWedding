using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyWedding.Data;
using MyWedding.Models;
using MyWedding.Models.Enums;

namespace MyWedding.Controllers
{
    public class GuestController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public GuestController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        // Custom methods here
     }
}
