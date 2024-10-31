using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using StudentPortal.Data;
using StudentPortal.Models;
using System.Reflection.Emit;

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

        // GET method to retrieve the subject details by EDPCode
        [HttpGet]
        public IActionResult AddSchedule(int? edpCode)
        {
            if (edpCode == null)
            {
                return View(); // Initial view to enter EDPCode.
            }

            var edpfind = _studb.SubjectInfo
                .Include(s => s.Schedule)
                .FirstOrDefault(s => s.EdpCode == edpCode);

            if (edpfind == null)
            {
                ViewBag.ErrorMessage = "No schedule found for the given EDPCode.";
                return View();
            }

            if(edpfind.EdpCode != edpCode)
            {
                ViewBag.ErrorMessage = "No edpcode found.";
                return View();

            }
            if (edpfind != null)
            {
                TimeOnly startTime = new TimeOnly(06, 30);  // 08:30
                TimeOnly endTime = new TimeOnly(06, 30);    // 10:00

                // Format as HH:mm
                ViewBag.StartTimeFormatted = startTime.ToString("HH:mm");
                ViewBag.EndTimeFormatted = endTime.ToString("HH:mm");
                
                    var schedfind = _studb.ScheduleInfo
                .Include(s => s.Subject).Select(s => new Schedule
                {



                    SubEdpCode = edpfind.EdpCode,
                    SubjCode = edpfind.SubjCode,
                    description = edpfind.Descript,
                    starttime = startTime,
                    endtime = endTime
                }).FirstOrDefault();
                
                if (schedfind == null) {
                    return View();
                }
                Schedule? schedule = schedfind;
                return View(schedule);
            }



            return View();
             // Populate view with schedule details.
        }

        [HttpPost]
        
        public IActionResult AddSchedule(Schedule sched)
        {
            using (var transaction = _studb.Database.BeginTransaction())
            {
                if (ModelState.IsValid)
                {
                    _studb.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ScheduleInfo ON");

                    _studb.ScheduleInfo.Add(sched);
                    _studb.SaveChanges();
                    _studb.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ScheduleInfo OFF");
                    transaction.Commit();
                    return RedirectToAction("ScheduleList");
                }
                return View();
            }
            


        }




    }
}
