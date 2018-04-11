using Microsoft.AspNetCore.Mvc;
using MyWedding.Data;

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
