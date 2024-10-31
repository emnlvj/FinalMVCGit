using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;

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
            var subject = _studb.SubjectInfo
                .Include(pr => pr.PreRequisite)
                .ToList();

            return View(subject);
            
        }

            public IActionResult AddSubject()
        {

            return View();
        }
        [HttpPost]
        public IActionResult AddSubject(Subject studsub)
        {
            

                _studb.SubjectInfo.Add(studsub);
                _studb.SaveChanges();
                return RedirectToAction("SubjectSummary");

            
        }

        public IActionResult EditSubject(string? subjcode)
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


        [HttpPost]
        public IActionResult EditSubject(Subject? subjectobj)
        {
            if (subjectobj == null || string.IsNullOrEmpty(subjectobj.SubjCode))
            {
                ViewBag.Message = "Subject not found";
                return View(subjectobj);  // Return the view with the original object
            }

            if (ModelState.IsValid)
            {
                // Retrieve the subject from the database
                var subjectToUpdate = _studb.SubjectInfo.Find(subjectobj.SubjCode);

                if (subjectToUpdate != null)
                {
                    // Update the fields
                    subjectToUpdate.Descript = subjectobj.Descript;
                    subjectToUpdate.Units = subjectobj.Units;
                    subjectToUpdate.Offering = subjectobj.Offering;
                    subjectToUpdate.CatCourse = subjectobj.CatCourse;
                    subjectToUpdate.EdpCode = subjectobj.EdpCode;
                    subjectToUpdate.CurrYear = subjectobj.CurrYear;

                    // Save changes to the database
                    _studb.SubjectInfo.Update(subjectToUpdate);
                    _studb.SaveChanges();

                    return RedirectToAction("SubjectSummary");
                }
                else
                {
                    ViewBag.Message = "Subject not found in the database.";
                }
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
            Subject? subobj = _studb.SubjectInfo.Find(subjcode);
            if (subobj == null)
            {
                ViewBag.Message = "Student not found";
                return RedirectToAction("Index");
            }


            _studb.SubjectInfo.Remove(subobj);
            _studb.SaveChanges();
            return RedirectToAction("SubjectSummary");

            
        }



    }
}
