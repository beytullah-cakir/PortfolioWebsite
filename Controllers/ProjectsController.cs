using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.Data;

namespace Portfolio.Controllers
{
    public class ProjectsController : Controller
    {
        public async Task<IActionResult> Index()
        {
            
            return View();
        }
    }
}
