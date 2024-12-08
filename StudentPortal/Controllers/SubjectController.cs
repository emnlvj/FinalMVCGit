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
        public IActionResult EditSubject(string? subjcode, string? courseobj)
        {
            if (subjcode == null)
            {
               
                return View();
            }
            if(courseobj  == null)
            {
               
                return View();
            }
            var subjcodefind = _studb.SubjectInfo
            .Include(s => s.PreRequisite)
                .FirstOrDefault(s => s.SubjCode == subjcode && s.CatCourse == courseobj);
            if (subjcodefind == null)
            {
                ViewBag.Message = "Subject not found";
                return View();
            }
            
            return View(subjcodefind);
        }

        [HttpGet]
        public JsonResult VerifySubjectCode(string subjcode, string currentSubjCode, Subject subjectobj)
        {
            var existingSubject = _studb.SubjectInfo.AsNoTracking().FirstOrDefault(s => s.SubjCode == subjectobj.SubjCode);
            var existingSubjectNot = _studb.SubjectInfo.AsNoTracking().FirstOrDefault(s => s.SubjCode != subjectobj.SubjCode);

            if (existingSubject != null)
            {
                bool exists = _studb.SubjectInfo.Any(s => s.SubjCode == subjcode && s.SubjCode != currentSubjCode);


                return Json(new { exists });
            }
            if (existingSubjectNot != null)
            {
                bool exists = _studb.SubjectInfo.Any(s => s.SubjCode == subjcode && s.SubjCode != currentSubjCode);

                return Json(new { exists });

            }
            return Json(new { });
        }


        [HttpPost]
        public IActionResult EditSubject(Subject? subjectobj, Subject? courseobj)
        {
            if (subjectobj == null || string.IsNullOrEmpty(subjectobj.SubjCode) && courseobj == null || string.IsNullOrEmpty(subjectobj.CatCourse))
            {
                ViewBag.Message = "Subject not found";
                return View(subjectobj);  // Return the view with the original object
            }

            // Retrieve existing subject
            var subjectToUpdate = _studb.SubjectInfo
                .Include(s => s.PreRequisite)
                .Include(s => s.Schedule)
                .Where(s => s.SubjCode == subjectobj.SubjCode).AsNoTracking().FirstOrDefault();

            var subjectToUpNotEqual = _studb.SubjectInfo
                .Include(s => s.PreRequisite)
                .Include(s => s.Schedule)
                .Where(s => s.SubjCode != subjectobj.SubjCode).AsNoTracking().FirstOrDefault();

            if (subjectToUpdate != null)
            {
                
                
                var newSubject = new Subject
                {
                    SubjCode = subjectobj.SubjCode,
                    Descript = subjectobj.Descript,
                    Units = subjectobj.Units,
                    Offering = subjectobj.Offering,
                    CatCourse = subjectobj.CatCourse,
                    CurrYear = subjectobj.CurrYear
                    
                };
               

                    _studb.SubjectInfo.Update(newSubject);
                    _studb.SaveChanges();
                    return RedirectToAction("SubjectSummary");
                


            }

            if(subjectToUpNotEqual != null)
            {
                
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
                    _studb.SaveChanges();

                    
                    _studb.ChangeTracker.Clear();
                    return RedirectToAction("SubjectSummary");
                

            }

            
            return View(subjectobj);
        }



        public IActionResult DeleteSubject(string? subjcode, string? courseobj)
        {
            if (subjcode == null)
            {

                return View();
            }
            var subjcodefind = _studb.SubjectInfo
           .Include(s => s.PreRequisite)
               .FirstOrDefault(s => s.SubjCode == subjcode && s.CatCourse == courseobj);
            if (subjcodefind == null)
            {
                ViewBag.Message = "Subject not found";
                return View();
            }
            return View(subjcodefind);
        }

        [HttpPost,ActionName("DeleteSubject")]
        public IActionResult DeleteSubjectPOST(string? subjcode, string? courseobj)
        {
            var subjectfind = _studb.SubjectInfo
          .Include(s => s.PreRequisite)
              .FirstOrDefault(s => s.SubjCode == subjcode && s.CatCourse == courseobj);

            if (subjectfind == null)
            {
                ViewBag.Message = "No subject found";
                return View();
            }
            
                _studb.SubjectInfo.Remove(subjectfind);
                try
                {
                     _studb.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    // Handle cases where constraints are not met, like a dangling foreign key.
                    // You may log the exception or handle it in a user-friendly way.
                    ViewBag.Message =  "Unable to delete subject. It may have related data.";
                    return View(subjectfind); // Or redirect with an error message
                }


                return RedirectToAction("SubjectSummary");
        }



    }
}
