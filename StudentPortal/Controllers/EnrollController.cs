using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using StudentPortal.Data;
using StudentPortal.Models;

namespace StudentPortal.Controllers
{
    public class EnrollController : Controller
    {
        private readonly StudentEntryDbContext _studb;

        public EnrollController(StudentEntryDbContext studdb)
        {
            _studb = studdb;
        }

        public IActionResult EnrollmentForm()
        {
            

            return View();
        }

        [HttpPost]
        public IActionResult EnrollmentForm([FromBody] StudentEnrollmentViewModel formData)
        {
            if (formData == null || formData.Id == 0 || formData.Schedules == null || !formData.Schedules.Any())
            {
                return Json(new { success = false, message = "Invalid form data" });
            }

            try
            {
                foreach (var schedule in formData.Schedules)
                {
                    var enrollment = new StudentEnrollment
                    {
                        Id = formData.Id,
                        SubEdpCode = schedule.SubEdpCode
                    };

                    _studb.StudentEnrollment.Add(enrollment);
                }

                _studb.SaveChanges();

                return Json(new { success = true, message = "Enrollment successful!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public IActionResult EnrollmentReview()
        {


            return View();
        }

        [HttpGet]
        public IActionResult GetStudentEnrollment(int studentId)
        {
           StudentWithScheduleViewModel viewModel = new StudentWithScheduleViewModel();
            var student = _studb.StudentInfo.Include(s => s.StudentEnrollment)
                                            .ThenInclude(e => e.Schedule)
                                            .FirstOrDefault(s => s.Id == studentId);



            if (student == null)
            {
                return NotFound(new { message = "Student not found." });
            }

            var schedules = student.StudentEnrollment.Select(e => new
            {
                e.Schedule.SubEdpCode,
                e.Schedule.SubjCode,
                e.Schedule.starttime,
                e.Schedule.endtime,
                e.Schedule.days,
                e.Schedule.roomnum
            });

            return Json(new
            {
                student = new
                {
                    name = $"{student.LName}, {student.FName} {student.MName}".Trim(),
                    course = student.Course,
                    year = student.Year
                },
                schedules
               
            });
        }








        [HttpGet]
        public IActionResult GetStudentById(int idNumber)
        {
            var student = _studb.StudentInfo
                .FirstOrDefault(s => s.Id == idNumber);

            if (student == null)
            {
                return NotFound(new { message = "Student not found" });
            }

            return Json(new
            {
                name = $"{student.LName}, {student.FName} {student.MName}".Trim(),
                course = student.Course,
                year = student.Year
            });
        }

        [HttpGet]
        public IActionResult GetScheduleByEdpCode(int edpCode)
        {
            var schedule = _studb.ScheduleInfo
                .FirstOrDefault(s => s.SubEdpCode == edpCode);

            if (schedule == null)
            {
                return NotFound(new { message = "Schedule not found." });
            }
            
            return Json(new
            {
                edpCode = schedule.SubEdpCode,
                subjectCode = schedule.SubjCode,
                startTime = schedule.starttime.ToString("hh\\:mm"),
                endTime = schedule.endtime.ToString("hh\\:mm"),
                days = schedule.days,
                room = schedule.roomnum
               
            });


            
        }

        [HttpGet]
        public IActionResult GetUnitsBySubjectCode(string subjectCode)
        {
            var subject = _studb.SubjectInfo.FirstOrDefault(s => s.SubjCode == subjectCode);
            if (subject == null)
            {
                return NotFound(new { message = "Subject not found." });
            }

            return Ok(new { units = subject.Units });
        }





    }
}
