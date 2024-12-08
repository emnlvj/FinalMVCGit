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
                schedule.description = schedule.SubjCode != null ? schedule.description : "To be edited";
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
                ViewBag.ErrorMessage = "No subject found for the given input.";
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

            sched.description = sched.SubjCode != null ? sched.description : "";

            if (sched.SubEdpCode == 0)
            {
                ViewBag.ErrorMessage = "Invalid EDP Code.";
                return View(sched);
            }

            var existingSchedule = _studb.ScheduleInfo.FirstOrDefault(s => s.SubEdpCode == sched.SubEdpCode);
            if (existingSchedule != null)
            {
                ViewBag.ErrorMessage = "EDP Code already exists.";
                return View(sched);
            }


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

        [HttpGet("GetSubjectDescription")]
        public IActionResult GetSubjectDescription(string subjCode)
        {
            if (!string.IsNullOrEmpty(subjCode))
            {
                var subject = _studb.SubjectInfo.SingleOrDefault(s => s.SubjCode == subjCode);
                if (subject != null)
                {
                    return Json(new { description = subject.Descript });
                }
            }
            return Json(new { description = "Subject description not found." });
        }

        [HttpGet("EditSchedule/{edpcode:int?}")]
        public IActionResult EditSchedule(int? edpcode, string subjCode)  // Accept subjCode as parameter
        {
            if (edpcode == null)
            {
                return View();
            }

            // Find the schedule using the provided EDP code
            Schedule? schedfind = _studb.ScheduleInfo
                .Include(s => s.Subject)  // Include the related Subject to access SubjDesc
                .FirstOrDefault(s => s.SubEdpCode == edpcode);

            if (schedfind == null)
            {
                ViewBag.ErrorMessage = "Schedule not found";
                return View(schedfind);
            }

            // If subjCode is provided, find the subject description
            if (!string.IsNullOrEmpty(subjCode))
            {
                var subject = _studb.SubjectInfo.SingleOrDefault(s => s.SubjCode == subjCode);
                if (subject != null)
                {
                    return Json(new { description = subject.Descript });  // Return the description as JSON
                }
                else
                {
                    return Json(new { description = "Subject description not found." });  // Return a default message if subject is not found
                }
            }


            // Return the view with the found schedule
            ViewBag.EdpCode = edpcode;
            return View(schedfind);
        }

        [HttpGet]
        public JsonResult VerifyEdpCode(int edpCode, int currentEdpCode, Schedule schedule)
        {
            var existingSchedule = _studb.ScheduleInfo.AsNoTracking().FirstOrDefault(s => s.SubEdpCode == schedule.SubEdpCode);
            var existingScheduleNot = _studb.ScheduleInfo.AsNoTracking().FirstOrDefault(s => s.SubEdpCode != schedule.SubEdpCode);

            if (existingSchedule != null)
            {
                bool exists = _studb.ScheduleInfo.Any(s => s.SubEdpCode == edpCode && s.SubEdpCode != currentEdpCode);


                return Json(new { exists });
            }
            if (existingScheduleNot != null)
            {
                bool exists = _studb.ScheduleInfo.Any(s => s.SubEdpCode == edpCode && s.SubEdpCode != currentEdpCode);

                return Json(new { exists });

            }
            return Json(new { });
        }

        [HttpPost("EditSchedule")]
        public IActionResult EditSchedule(Schedule? schedule)
        {
            if (schedule == null || schedule.SubEdpCode == 0)
            {
                ViewBag.ErrorMessage = "Schedule not found";
                return View(schedule);
            }

            schedule.AMPM = GetTimeOfDay(schedule.starttime, schedule.endtime);
            schedule.status = schedule.SubjCode != null ? "Active" : "InActive";

            if (schedule.SubEdpCode == 0)
            {
                ViewBag.ErrorMessage = "Invalid EDP Code.";
                return View(schedule);
            }

            if (schedule.SubjCode != null)
            {
                var subject = _studb.SubjectInfo.SingleOrDefault(s => s.SubjCode == schedule.SubjCode);
                schedule.description = subject?.Descript ?? "";
            }

            if (!string.IsNullOrEmpty(schedule.SubjCode))
            {
                var subjectExists = _studb.SubjectInfo.Any(s => s.SubjCode == schedule.SubjCode);


                if (!subjectExists)
                {
                    schedule.SubjCode = null;
                }
            }

            var existingSchedule = _studb.ScheduleInfo.AsNoTracking().FirstOrDefault(s => s.SubEdpCode == schedule.SubEdpCode);
            var existingScheduleNot = _studb.ScheduleInfo.AsNoTracking().FirstOrDefault(s => s.SubEdpCode != schedule.SubEdpCode);

            var existingSchedulesNot = _studb.ScheduleInfo
                .Where(s => s.SubEdpCode != schedule.SubEdpCode).AsNoTracking()
                .ToList();

            var schedulesToCheck = _studb.ScheduleInfo
                    .Where(s => s.SubEdpCode != schedule.SubEdpCode)
                    .AsNoTracking()
                    .ToList();

            if (existingSchedule != null)
            {

                // Check if starttime or endtime have changed
                bool isStartTimeChanged = existingSchedule.starttime != schedule.starttime;
                bool isEndTimeChanged = existingSchedule.endtime != schedule.endtime;

                // Only check for conflicts if starttime or endtime have changed
                if ((isStartTimeChanged || isEndTimeChanged || !schedulesToCheck.Contains(existingSchedule))
                    && _conflictChecker.HasConflict(schedule, schedulesToCheck))
                {
                    ViewBag.ErrorMessage = "The updated schedule conflicts with an existing schedule.";
                    return View(schedule);
                }


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
                    _studb.ScheduleInfo.Update(newSched);
                    _studb.SaveChanges();

                    _studb.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ScheduleInfo OFF");
                    transaction.Commit();
                    _studb.ChangeTracker.Clear();
                    return RedirectToAction("ScheduleList");
                }
            }

            if (existingScheduleNot != null)
            {
                bool overlapsWithOriginal = schedule.starttime < existingSchedule?.endtime && schedule.endtime > existingSchedule?.starttime;
                bool hasChanges =
                              existingScheduleNot.roomnum != schedule.roomnum ||
                               existingScheduleNot.days != schedule.days ||
                               existingScheduleNot.starttime != schedule.starttime ||
                               existingScheduleNot.endtime != schedule.endtime;


                if (hasChanges && !overlapsWithOriginal && _conflictChecker.HasConflict(schedule, existingSchedulesNot))
                {
                    ViewBag.ErrorMessage = "The updated schedule conflicts with an existing schedule.";
                    return View(schedule);
                }
                _studb.ScheduleInfo.Remove(existingScheduleNot);
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
                    _studb.ChangeTracker.Clear();
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

        [HttpGet
            ]
        public IActionResult DeleteSchedule(int? edpCode)
        {
            if (edpCode == null)
            {

                return View();
            }
            Schedule? schedfind = _studb.ScheduleInfo.Find(edpCode);
            if (schedfind == null)
            {
                return View();
            }
            return View(schedfind);
        }

        [HttpPost, ActionName("DeleteSchedule")]
        public IActionResult DeleteSchedulePOST(int? edpCode)
        {
            var schedule = _studb.ScheduleInfo.Find(edpCode);

            if (schedule == null)
            {
                ViewBag.ErrorMessage = "No schedule found";
                return View();
            }
            schedule.description = schedule.SubjCode != null ? schedule.description : "";
            _studb.ScheduleInfo.Remove(schedule);
            try
            {
                _studb.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                // Handle cases where constraints are not met, like a dangling foreign key.
                // You may log the exception or handle it in a user-friendly way.
                ViewBag.Message = "Unable to delete subject. It may have related data.";
                return View(schedule); // Or redirect with an error message
            }


            return RedirectToAction("ScheduleList");
        }




    }
}
