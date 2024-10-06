using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;
using System.Diagnostics;

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

        public IActionResult PreReqAdd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PreReqAdd(PreRequisite prerequisite)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if the subject code exists in the Subject table
                    if (!_studb.SubjectInfo.Any(s => s.SubjCode == prerequisite.SubjCode))
                    {
                        ModelState.AddModelError("SubjCode", "The Subject Code does not exist.");
                        return View(prerequisite);
                    }

                    // Check if the prerequisite Subject is valid


                    // If everything is valid, create a new instance
                    var newPrerequisite = new PreRequisite
                    {
                        PreSubjCode = ViewBag.PreSubjCode ?? prerequisite.PreSubjCode,
                        PreDescript = ViewBag.PreDescript ??prerequisite.PreDescript,
                        PreUnits = ViewBag.PreUnits ?? prerequisite.PreUnits,
                        SubjCode = ViewBag.SubjectCode ?? prerequisite.SubjCode

                    };

                    // Add to the database and save
                    _studb.PreSubjectInfo.Add(newPrerequisite);
                    _studb.SaveChanges();

                    return RedirectToAction("PreReqList"); // Redirect to list
                }
                catch (IOException ex)
                {
                    // Log the exception (optional)
                    // For example: _logger.LogError(ex, "An error occurred while saving the prerequisite.");

                    ModelState.AddModelError("", "An error occurred while processing your request. Please try again.");
                    return View(prerequisite);
                }
            }

            return View();        }
    }
}
