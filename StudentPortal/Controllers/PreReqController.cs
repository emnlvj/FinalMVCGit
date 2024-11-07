using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;
using System.Diagnostics;
using System.Reflection.Emit;

namespace StudentPortal.Controllers
{
    public class PreReqController : Controller
    {
        private readonly StudentEntryDbContext _studb;

        public PreReqController(StudentEntryDbContext studdb)
        {
            _studb = studdb;
        }
        public IActionResult PreReqList()
        {
            var preRequisites = _studb.PreSubjectInfo
                .Include(pr => pr.Subject)
                .ToList();

            return View(preRequisites);


        }

        public IActionResult PreReqAdd(string presubjcode)
        {
            // Check if presubjcode is null and return the view to enter the subject code.
            if (string.IsNullOrEmpty(presubjcode))
            {
                return View(); // Show the view to enter the Subject Code
            }

            // Find the Subject by the given subjCode (presubjcode)
            var subjcodefind = _studb.SubjectInfo
                .Include(s => s.PreRequisite)  // Include the related PreRequisite information
                .FirstOrDefault(s => s.SubjCode == presubjcode);

            // If the Subject is not found, show an error message
            if (subjcodefind == null)
            {
                ViewBag.ErrorMessage = "No prerequisite found for the given code.";
                return View();
            }

            var schedule = new PreRequisite
            {
                SubjCode = subjcodefind.SubjCode
            };
            // If Subject is found, create a new PreRequisite object and pass it to the view
            

            return View(schedule); // Pass the PreRequisite object to the view
        }


        [HttpPost]
        public IActionResult PreReqAdd(PreRequisite prerequisite)
        {
               
                _studb.PreSubjectInfo.Add(prerequisite);
                _studb.SaveChanges();
                return RedirectToAction("PreReqList");
            
        }
    }
}
