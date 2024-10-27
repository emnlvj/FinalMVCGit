using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;

namespace StudentPortal.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly StudentEntryDbContext _studb;

        public ScheduleController(StudentEntryDbContext studdb)
        {
            _studb = studdb;
        }
        public IActionResult ScheduleList()
        {
            var sched = _studb.ScheduleInfo
                .Include(pr => pr.Subject)
                .ToList();

            return View(sched);
        }
    }
}
