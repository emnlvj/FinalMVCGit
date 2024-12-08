using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;
using System.Reflection.Emit;



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

                if (ModelState.IsValid)
                {

                    using (var transaction = _studb.Database.BeginTransaction())
                    {
                        _studb.Database.ExecuteSqlRaw("SET IDENTITY_INSERT StudentInfo ON");
                        _studb.StudentInfo.Add(studobj);
                        _studb.SaveChanges();
                        _studb.Database.ExecuteSqlRaw("SET IDENTITY_INSERT StudentInfo OFF");

                        transaction.Commit();
                        return RedirectToAction("StudentList");
                    }

                }
                return View();
        }
        [HttpGet("EditStudent/{id:int?}")]
        public IActionResult EditStudent(int? id)
        {
            if (id == null)
            {
                
                return View();
            }
            Student? studfind = _studb.StudentInfo  // Include the related Subject to access SubjDesc
                .FirstOrDefault(s => s.Id== id);
            if (studfind == null)
            {
                ViewBag.ErrorMessage = "Student not found";
                return View(studfind);
            }

            return View(studfind);
        }

        [HttpGet]
        public JsonResult VerifyStudID(int id, int currentID, Student studobj)
        {
            var existingStudent = _studb.StudentInfo.AsNoTracking().FirstOrDefault(s => s.Id == studobj.Id);
            var existingStudentNot = _studb.StudentInfo.AsNoTracking().FirstOrDefault(s => s.Id != studobj.Id);

            if (existingStudent != null)
            {
                bool exists = _studb.StudentInfo.Any(s => s.Id == id && s.Id != currentID);


                return Json(new { exists });
            }
            if (existingStudentNot != null)
            {
                bool exists = _studb.StudentInfo.Any(s => s.Id == id && s.Id != currentID);

                return Json(new { exists });

            }
            return Json(new { });
        }

        [HttpPost("EditStudent")]

        public IActionResult EditStudent(Student? studobj)
        {
            
                    if (studobj == null)
                    {
                        ViewBag.Message = "Student not found";
                        return View();
                    }

                    if (studobj.Id == 0)
                    {
                        ViewBag.ErrorMessage = "Invalid ID.";
                        return View(studobj);
                    }

                    var existingStudent = _studb.StudentInfo.AsNoTracking().FirstOrDefault(s => s.Id == studobj.Id);
                    var existingStudentNot = _studb.StudentInfo.AsNoTracking().FirstOrDefault(s => s.Id != studobj.Id);

              if (existingStudent != null) {

                    
                    

                    var newStudent = new Student
                    {
                        Id = studobj.Id,
                        Course = studobj.Course,
                        Status = studobj.Status,
                        FName = studobj.FName,
                        MName = studobj.MName,
                        LName = studobj.LName,
                        Remarks = studobj.Remarks,
                        Year = studobj.Year
                    };

                    using (var transaction = _studb.Database.BeginTransaction())
                    {
                    _studb.Database.ExecuteSqlRaw("SET IDENTITY_INSERT StudentInfo ON");

                    _studb.StudentInfo.Update(newStudent);
                    _studb.SaveChanges();

                    _studb.Database.ExecuteSqlRaw("SET IDENTITY_INSERT StudentInfo OFF");

                    transaction.Commit();
                    return RedirectToAction("StudentList");
                    }
              }

             if (existingStudentNot != null)
             {
                    _studb.StudentInfo.Remove(existingStudentNot);
                    _studb.SaveChanges();


                    var newStudent = new Student
                    {
                        Id = studobj.Id,
                        Course = studobj.Course,
                        Status = studobj.Status,
                        FName = studobj.FName,
                        MName = studobj.MName,
                        LName = studobj.LName,
                        Remarks = studobj.Remarks,
                        Year = studobj.Year
                    };

                    using (var transaction = _studb.Database.BeginTransaction())
                    {
                    _studb.Database.ExecuteSqlRaw("SET IDENTITY_INSERT StudentInfo ON");
                    _studb.StudentInfo.Add(newStudent);
                    _studb.SaveChanges();

                    _studb.Database.ExecuteSqlRaw("SET IDENTITY_INSERT StudentInfo OFF");
                    transaction.Commit();
                    _studb.ChangeTracker.Clear();
                    return RedirectToAction("StudentList");
                    }

            }
            return View(studobj);
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

            
                try
                {

                    // Remove the student object from the database
                    _studb.StudentInfo.Remove(studobj);
                    _studb.SaveChanges();

                    // Find all students with a count greater than the deleted student's count
              

                    

                    // Save changes for updated students
                    _studb.SaveChanges();

                    return RedirectToAction("StudentList");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    // Handle concurrency conflict
                    ViewBag.Message = "Concurrency conflict detected. The data may have changed or been deleted.";
                    
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Handle any other errors
                    ViewBag.Message = "An error occurred while deleting the student.";
                
                    return RedirectToAction("Index");
                }
            
        }

        



    }
}
