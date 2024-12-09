using Microsoft.AspNetCore.Mvc;

namespace StudentPortal.Controllers
{
    public class EnrollController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult EnrollmentForm()
        {
            return View();
        }
    }
}
