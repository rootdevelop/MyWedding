using Microsoft.AspNetCore.Mvc;
using MyWedding.Data;


namespace MyWedding.Controllers
{
    public class AdminController : Controller
    {
       private readonly ApplicationDbContext _dbContext;

       public AdminController(ApplicationDbContext dbContext)
       {
           _dbContext = dbContext;
       }

       // Custom methods here
    }
}
