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
       
        public IActionResult AddSubject()
        {

            return View();
        }
        [HttpPost]
        public IActionResult AddSubject(Subject studsub)
        {


            if (ModelState.IsValid)
            {

                _studb.SubjectInfo.Add(studsub);
                _studb.SaveChanges();
                return RedirectToAction("SubjectList");


            }

            return View(studsub);


        }

      

        
    }
}
