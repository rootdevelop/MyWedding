using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWedding.Data;
using MyWedding.Models;
using MyWedding.Models.Enums;


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
