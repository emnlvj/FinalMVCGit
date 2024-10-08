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

                    if (!_studb.SubjectInfo.Any(s => s.SubjCode == prerequisite.SubjCode))
                    {
                        ModelState.AddModelError("SubjCode", "The Subject Code does not exist.");
                        return View(prerequisite);
                    }
                    _studb.PreSubjectInfo.Add(prerequisite);
                    _studb.SaveChanges();

                    return RedirectToAction("PreReqList"); // Redirect to list
                }
                catch (IOException ex)
                {
                    ModelState.AddModelError("", "An error occurred while processing your request. Please try again.");
                    return View(prerequisite);
                }
            }
            if (!ModelState.IsValid) {
                if (!_studb.SubjectInfo.Any(s => s.SubjCode == prerequisite.SubjCode))
                {
                    ModelState.AddModelError("SubjCode", "The Subject Code does not exist.");
                    return View(prerequisite);
                }

                
                _studb.PreSubjectInfo.Add(prerequisite);
                _studb.SaveChanges();

                return RedirectToAction("PreReqList");
            }
            return View();       
        }
    }
}
