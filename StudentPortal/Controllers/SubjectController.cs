using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;
using System.Data;

namespace StudentPortal.Controllers
{
    public class SubjectController : Controller
    {
        private readonly StudentEntryDbContext _studb;

        public SubjectController(StudentEntryDbContext studdb)
        {
            _studb = studdb;
        }
        public IActionResult SubjectList(string subjCode)
        {
           
            SubjectWithPreRequisitesViewModel viewModel = new SubjectWithPreRequisitesViewModel();

            if (!string.IsNullOrEmpty(subjCode))
            {
                // Fetch the Subject along with its PreRequisites based on the provided SubjCode
                var subject = _studb.SubjectInfo
                    .Include(s => s.PreRequisite)  // Include PreRequisite info
                    .FirstOrDefault(s => s.SubjCode == subjCode);

                if (subject != null)
                {
                    viewModel.Subject = subject;
                    viewModel.PreRequisites = subject.PreRequisite.ToList();
                }
                else
                {
                    ViewBag.ErrorMessage = "No subject found for the given code.";
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Please provide a valid Subject Code.";
            }

            return View(viewModel);
        }
        public IActionResult SubjectSummary()
        {
            var subjects = _studb.SubjectInfo
            .Include(s => s.Schedule)
            .Include(s => s.PreRequisite)
            .ToList();

            return View(subjects);
            
            
        }

        public IActionResult AddSubject()
        {
            // Retrieve the PreSubjCode values from all PreRequisites to display in the dropdown
            

            // Pass an empty Subject object to the view for form binding
            return View();
        }

        [HttpPost]
        public IActionResult AddSubject(Subject studsub)
        {
            
                _studb.SubjectInfo.Add(studsub);
                _studb.SaveChanges();
                return RedirectToAction("SubjectSummary");
            

           
        }


        [HttpGet]
        public IActionResult EditSubject(string? subjcode)
        {
            if (subjcode == null)
            {

                return View();
            }
            var subjcodefind = _studb.SubjectInfo
            .Include(s => s.PreRequisite)
                .FirstOrDefault(s => s.SubjCode == subjcode);
            if (subjcodefind == null)
            {
                return View();
            }
            
            return View(subjcodefind);
        }


        [HttpPost]
        public IActionResult EditSubject(Subject? subjectobj)
        {
            if (subjectobj == null || string.IsNullOrEmpty(subjectobj.SubjCode))
            {
                ViewBag.Message = "Subject not found";
                return View(subjectobj);  // Return the view with the original object
            }

            // Retrieve existing subject
            var subjectToUpdate = _studb.SubjectInfo
                .Include(s => s.PreRequisite)
                .Include(s => s.Schedule)
                .Where(s => s.SubjCode == subjectobj.SubjCode).FirstOrDefault();

            var subjectToUpNotEqual = _studb.SubjectInfo
                .Include(s => s.PreRequisite)
                .Include(s => s.Schedule)
                .Where(s => s.SubjCode != subjectobj.SubjCode).FirstOrDefault();

            var prereqlist = _studb.PreSubjectInfo
                  .Include(s => s.Subject)
                  .Where(s => s.SubjCode == subjectobj.SubjCode).AsTracking().ToList();

            var prereqlistnot = _studb.PreSubjectInfo
                  .Include(s => s.Subject)
                  .Where(s => s.SubjCode != subjectobj.SubjCode).AsTracking().ToList();

            var schedlist = _studb.ScheduleInfo
                  .Include(s => s.Subject)
                  .Where(s => s.SubjCode == subjectobj.SubjCode).AsTracking().ToList();

            var schedlistnot = _studb.ScheduleInfo
                  .Include(s => s.Subject)
                  .Where(s => s.SubjCode != subjectobj.SubjCode).AsTracking().ToList();


            if (subjectToUpdate != null)
            {
                _studb.ScheduleInfo.RemoveRange(schedlist);
                _studb.PreSubjectInfo.RemoveRange(prereqlist);
                _studb.SubjectInfo.Remove(subjectToUpdate);
                _studb.SaveChanges();
                var newSubject = new Subject
                {
                    SubjCode = subjectobj.SubjCode,
                    Descript = subjectobj.Descript,
                    Units = subjectobj.Units,
                    Offering = subjectobj.Offering,
                    CatCourse = subjectobj.CatCourse,
                    CurrYear = subjectobj.CurrYear
                    
                };
                _studb.SubjectInfo.Add(newSubject);
                _studb.SaveChanges();
                
                    foreach (var preq in prereqlist)
                    {
                        var newPreReq = new PreRequisite
                        {
                            PreSubjCode = preq.PreSubjCode,
                            PreDescript = preq.PreDescript,
                            PreUnits = preq.PreUnits,
                            SubjCode = subjectobj.SubjCode
                        };
                        _studb.PreSubjectInfo.Add(newPreReq);
                        _studb.SaveChanges();
                }

                foreach (var sched in schedlist)
                {
                    var newSched = new Schedule
                    {
                        SubEdpCode = sched.SubEdpCode,
                        description = sched.description,
                        starttime = sched.starttime,
                        endtime = sched.endtime,
                        section = sched.section,
                        roomnum = sched.roomnum,
                        AMPM = sched.AMPM,
                        curryear = sched.curryear,
                        days = sched.days,
                        status = sched.status,
                        SubjCode = subjectobj.SubjCode
                    };
                    using (var transaction = _studb.Database.BeginTransaction())
                    {
                        _studb.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ScheduleInfo ON");
                        _studb.ScheduleInfo.Add(newSched);
                        _studb.SaveChanges();
                        _studb.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ScheduleInfo OFF");
                        transaction.Commit();
                        
                    }
                    
                }
                return RedirectToAction("SubjectSummary");

            }

            if(subjectToUpNotEqual != null)
            {



                _studb.ScheduleInfo.RemoveRange(schedlistnot);
                _studb.PreSubjectInfo.RemoveRange(prereqlistnot);
                _studb.SubjectInfo.Remove(subjectToUpNotEqual);
                _studb.SaveChanges();
                // Add the new subject with updated SubjCode
                var newSubject = new Subject
                    {
                    SubjCode = subjectobj.SubjCode,
                    Descript = subjectobj.Descript,
                    Units = subjectobj.Units,
                    Offering = subjectobj.Offering,
                    CatCourse = subjectobj.CatCourse,
                   
                    CurrYear = subjectobj.CurrYear,
                    
                    };

                    _studb.SubjectInfo.Add(newSubject);
                    _studb.SaveChanges(); // Save new subject

                foreach (var preq in prereqlistnot)
                { 
                    var newPreReq = new PreRequisite
                    {
                        PreSubjCode = preq.PreSubjCode,
                        PreDescript = preq.PreDescript,
                        PreUnits = preq.PreUnits,
                        SubjCode = subjectobj.SubjCode
                    };
                    _studb.PreSubjectInfo.Add(newPreReq);
                    _studb.SaveChanges();
                }

                foreach (var sched in schedlistnot)
                {
                    var newSched = new Schedule
                    {
                        SubEdpCode = sched.SubEdpCode,
                        description = sched.description,
                        starttime = sched.starttime,
                        endtime = sched.endtime,
                        section = sched.section,
                        roomnum = sched.roomnum,
                        AMPM = sched.AMPM,
                        curryear = sched.curryear,
                        days = sched.days,
                        status = sched.status,
                        SubjCode = subjectobj.SubjCode
                    };
                    
                    using (var transaction = _studb.Database.BeginTransaction())
                    {
                        _studb.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ScheduleInfo ON");
                        _studb.ScheduleInfo.Add(newSched);
                        _studb.SaveChanges();
                        _studb.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ScheduleInfo OFF");
                        transaction.Commit();

                    }
                    _studb.ChangeTracker.Clear();
                }
                return RedirectToAction("SubjectSummary");

            }

            
            return View(subjectobj);
        }



        public IActionResult DeleteSubject(string? subjcode)
        {
            if (subjcode == null)
            {

                return View();
            }
            Subject? subfind = _studb.SubjectInfo.Find(subjcode);
            if (subfind == null)
            {
                return View();
            }
            return View(subfind);
        }

        [HttpPost,ActionName("DeleteSubject")]
        public IActionResult DeleteSubjectPOST(string? subjcode)
        {
            var subject = _studb.SubjectInfo.Find(subjcode);

            if (subject == null)
            {
                ViewBag.Message = "No subject found";
                return View();
            }
            
                _studb.SubjectInfo.Remove(subject);
                try
                {
                     _studb.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    // Handle cases where constraints are not met, like a dangling foreign key.
                    // You may log the exception or handle it in a user-friendly way.
                    ViewBag.Message =  "Unable to delete subject. It may have related data.";
                    return View(subject); // Or redirect with an error message
                }


                return RedirectToAction("SubjectSummary");
        }



    }
}
