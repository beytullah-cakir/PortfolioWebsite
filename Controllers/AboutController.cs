using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Portfolio.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
