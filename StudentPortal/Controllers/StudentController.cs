using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;

namespace StudentPortal.Controllers
{
	public class StudentController : Controller
	{
		private readonly StudentEntryDbContext _studb;
        public StudentController(StudentEntryDbContext studdb)
        {
			_studb = studdb;
        }
        public IActionResult StudentList(string searchTerm)
		{
            
            List<Student> objStudentList = _studb.StudentInfo.ToList();
            
           
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                objStudentList = objStudentList
                    .Where(s => s.Id.ToString().Equals(searchTerm, StringComparison.OrdinalIgnoreCase)
                             || s.FName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                             || s.LName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                             || s.MName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            
            ViewBag.SearchTerm = searchTerm;
            return View(objStudentList);
        }

        public IActionResult AddStudent()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult AddStudent(Student studobj)
        {
            using (var transaction = _studb.Database.BeginTransaction())
            {
                
                
                if (ModelState.IsValid)
                {
                    _studb.Database.ExecuteSqlRaw("SET IDENTITY_INSERT StudentInfo ON");
                    var maxCount = _studb.StudentInfo.Select(s => s.count).ToList().DefaultIfEmpty(0).Max();
                    studobj.count = maxCount + 1;

                    _studb.StudentInfo.Add(studobj);
                    _studb.SaveChanges();

                    _studb.Database.ExecuteSqlRaw("SET IDENTITY_INSERT StudentInfo OFF");

                    transaction.Commit();
                    return RedirectToAction("StudentList");
                }
                return View();
            }

        }
        [HttpGet]
        public IActionResult EditStudent(int? id)
        {
            if (id == null || id == 0)
            {
                
                return View();
            }
            Student? studfind = _studb.StudentInfo.Find(id);
            if (studfind == null)
            {
                return View();
            }
            return View(studfind);
        }

        [HttpPost]
        
        public IActionResult EditStudent(Student? studobj)
        {
            
            using (var transaction = _studb.Database.BeginTransaction())
            {
              
                
                if (ModelState.IsValid)
                {
                    if (studobj == null)
                    {
                        ViewBag.Message = "Student not found";
                        return View();
                    }

                    _studb.Database.ExecuteSqlRaw("SET IDENTITY_INSERT StudentInfo ON");
                    var originalStudent = _studb.StudentInfo.AsNoTracking().FirstOrDefault(s => s.Id == studobj.Id);
                    
                    if (originalStudent == null) {
                        return View();
                    }
                    
                    studobj.count = originalStudent.count;

                    _studb.StudentInfo.Update(studobj);
                    _studb.SaveChanges();
                   
                    _studb.Database.ExecuteSqlRaw("SET IDENTITY_INSERT StudentInfo OFF");
        
                    transaction.Commit();
                    return RedirectToAction("StudentList");
                }
                return View();
            }

        }
        [HttpGet]
        public IActionResult DeleteStudent(int? id)
        {
            if (id == null || id == 0)
            {

                return View();
            }
            Student? studfind = _studb.StudentInfo.Find(id);
            if (studfind == null)
            {
                return View();
            }
            return View(studfind);
        }

        [HttpPost, ActionName("DeleteStudent")]

        public IActionResult DeleteStudentPOST(int? id)
        {
            Student? studobj = _studb.StudentInfo.Find(id);
            if (studobj == null)
            {
                ViewBag.Message = "Student not found";
                return RedirectToAction("Index");
            }
           
            using (var transaction = _studb.Database.BeginTransaction())
            {
             
                    int deletedStudentCount = studobj.count;
                    _studb.Database.ExecuteSqlRaw("SET IDENTITY_INSERT StudentInfo ON");

                    _studb.StudentInfo.Remove(studobj);
                    _studb.SaveChanges();

                var studentsToUpdate = _studb.StudentInfo.Where(s => s.count > deletedStudentCount).ToList();

                foreach (var student in studentsToUpdate)
                {
                    student.count -= 1;
                    _studb.StudentInfo.Update(studobj);
                }


                _studb.SaveChanges();

                _studb.Database.ExecuteSqlRaw("SET IDENTITY_INSERT StudentInfo OFF");

                    transaction.Commit();
                    return RedirectToAction("StudentList");
                
                
            }

        }
    }
}
