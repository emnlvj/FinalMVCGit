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
            if (presubjcode == null)
            {
                return View(); // Initial view to enter EDPCode.
            }

            var subjcodefind= _studb.SubjectInfo
            .Include(s => s.PreRequisite)
                .FirstOrDefault(s => s.SubjCode == presubjcode);

            if (subjcodefind == null)
            {
                ViewBag.ErrorMessage = "No prerequisite found for the given code.";
                return View();
            }
            if (subjcodefind != null)
            {
                var prereq = _studb.PreSubjectInfo
               .Include(s => s.Subject).Select(s => new PreRequisite
               {
                   
                  
                   SubjCode = subjcodefind.SubjCode
               }).FirstOrDefault();


                PreRequisite? addpre = prereq;
                return View(addpre);
            }
            return View();
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
