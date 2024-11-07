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
        private readonly ScheduleConflictChecker _conflictChecker;
        public ScheduleController(StudentEntryDbContext studdb)
        {
            _studb = studdb;
            _conflictChecker = new ScheduleConflictChecker();
        }
        public IActionResult ScheduleList()
        {
            var sched = _studb.ScheduleInfo
                .Include(pr => pr.Subject)
                .ToList();

            foreach (var schedule in sched)
            {
                schedule.status = schedule.SubjCode != null ? "Active" : "InActive";
                schedule.description = schedule.SubjCode != null ? schedule.description: "To be edited";
                schedule.SubjCode = schedule.SubjCode != null ? schedule.SubjCode : "To be edited";

            }

            return View(sched);
        }

        // GET method to retrieve the subject details by EDPCode
        [HttpGet]
        public IActionResult AddSchedule(string edpCode)
        {
            if (edpCode == null)
            {
                return View(); // Initial view to enter EDPCode.
            }

            var edpfind = _studb.SubjectInfo
                .Include(s => s.Schedule)
                .FirstOrDefault(s => s.SubjCode == edpCode);

            if (edpfind == null)
            {
                ViewBag.ErrorMessage = "No schedule found for the given EDPCode.";
                return View();
            }

            
            if (edpfind != null)
            {
                TimeOnly startTime = new TimeOnly(06, 30);  // 08:30
                TimeOnly endTime = new TimeOnly(08, 30);    // 10:00

                // Format as HH:mm
                ViewBag.StartTimeFormatted = startTime.ToString("HH:mm");
                ViewBag.EndTimeFormatted = endTime.ToString("HH:mm");

                var schedule = new Schedule
                {
                    SubjCode = edpfind.SubjCode,
                    description = edpfind.Descript,
                    starttime = startTime,
                    endtime = endTime  // Set the SubjCode for the PreRequisite
                };

                schedule.status = schedule.SubjCode != null ? "Active" : "InActive";

                return View(schedule);
            }



            return View();
            // Populate view with schedule details.
        }

        [HttpPost]

        public IActionResult AddSchedule(Schedule sched)
        {
            var existingSchedules = _studb.ScheduleInfo.ToList();
            
            if (_conflictChecker.HasConflict(sched, existingSchedules))
            {
                ViewBag.Meessage = "The schedule conflicts with an existing schedule.";
                return View(sched);
            }

            sched.AMPM = GetTimeOfDay(sched.starttime, sched.endtime);

            sched.status = sched.SubjCode != null ? "Active" : "InActive";


            using (var transaction = _studb.Database.BeginTransaction())
            {
                _studb.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ScheduleInfo ON");
                _studb.ScheduleInfo.Add(sched);
                _studb.SaveChanges();
                _studb.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ScheduleInfo OFF");
                transaction.Commit();
                return RedirectToAction("ScheduleList");
            }
        }

        public IActionResult EditSchedule(int? edpcode)
        {
            if (edpcode == null)
            {
                return View();
            }

            // Find the schedule using the provided EDP code
            Schedule? schedfind = _studb.ScheduleInfo.Find(edpcode);
            if (schedfind == null)
            {
                ViewBag.Message = "Schedule not found";
                return View();
            }
            schedfind.status = schedfind.SubjCode != null ? "Active" : "InActive";
            // Return the view with the found schedule
            return View(schedfind);
        }

        [HttpPost]
        public IActionResult EditSchedule(Schedule? schedule)
        {
            if (schedule == null || schedule.SubEdpCode == 0)
            {
                ViewBag.Message = "Subject not found";
                return View(schedule);  // Return the view with the original object
            }

            schedule.AMPM = GetTimeOfDay(schedule.starttime, schedule.endtime);

            schedule.status = schedule.SubjCode != null ? "Active" : "InActive";

            var scheduleToUpdate = _studb.ScheduleInfo
                .Include(s => s.Subject)
                .Where(s => s.SubEdpCode == schedule.SubEdpCode).FirstOrDefault();

            var scheduleToUpdateNotEqual = _studb.ScheduleInfo
                .Include(s => s.Subject)
                .Where(s => s.SubEdpCode != schedule.SubEdpCode).FirstOrDefault();

            var existingSchedules = _studb.ScheduleInfo
                .Where(s => s.SubEdpCode == schedule.SubEdpCode)
                .ToList();

            var existingSchedulesNot = _studb.ScheduleInfo
                .Where(s => s.SubEdpCode != schedule.SubEdpCode)
                .ToList();

            if (scheduleToUpdate != null) {
                if (_conflictChecker.HasConflict(schedule, existingSchedules))
                {
                    ViewBag.Meessage = "The updated schedule conflicts with an existing schedule.";
                    return View(schedule);
                }
                _studb.ScheduleInfo.Remove(scheduleToUpdate);
                _studb.SaveChanges();

                var newSched = new Schedule
                {
                    SubEdpCode = schedule.SubEdpCode,
                    description = schedule.description,
                    starttime = schedule.starttime,
                    endtime = schedule.endtime,
                    section = schedule.section,
                    roomnum = schedule.roomnum,
                    AMPM = schedule.AMPM,
                    curryear = schedule.curryear,
                    days = schedule.days,
                    status = schedule.status,
                    SubjCode = schedule.SubjCode
                };
                using (var transaction = _studb.Database.BeginTransaction())
                {
                    _studb.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ScheduleInfo ON");
                    _studb.ScheduleInfo.Add(newSched);
                    _studb.SaveChanges();
                    _studb.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ScheduleInfo OFF");
                    transaction.Commit();
                    return RedirectToAction("ScheduleList");
                }
            }

            if (scheduleToUpdateNotEqual != null)
            {
                if (_conflictChecker.HasConflict(schedule, existingSchedulesNot))
                {
                    ViewBag.Meessage = "The updated schedule conflicts with an existing schedule.";
                    return View(schedule);
                }
                _studb.ScheduleInfo.Remove(scheduleToUpdateNotEqual);
                _studb.SaveChanges();

                var newSched = new Schedule
                {
                    SubEdpCode = schedule.SubEdpCode,
                    description = schedule.description,
                    starttime = schedule.starttime,
                    endtime = schedule.endtime,
                    section = schedule.section,
                    roomnum = schedule.roomnum,
                    AMPM = schedule.AMPM,
                    curryear = schedule.curryear,
                    days = schedule.days,
                    status = schedule.status,
                    SubjCode = schedule.SubjCode
                };
                using (var transaction = _studb.Database.BeginTransaction())
                {
                    _studb.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ScheduleInfo ON");
                    _studb.ScheduleInfo.Add(newSched);
                    _studb.SaveChanges();
                    _studb.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ScheduleInfo OFF");
                    transaction.Commit();
                    return RedirectToAction("ScheduleList");
                }
             }

            return View(schedule);
        }

        private string GetTimeOfDay(TimeOnly startTime, TimeOnly endTime)
        {
            // If either start time or end time is after 12:00 PM
            if (startTime.Hour >= 12 || endTime.Hour >= 12)
            {
                return "PM";
            }
            return "AM";
        }







    }
}
