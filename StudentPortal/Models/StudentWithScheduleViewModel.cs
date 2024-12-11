namespace StudentPortal.Models
{
    public class StudentWithScheduleViewModel
    {
        public Student Student { get; set; }
        public List<StudentEnrollment> Schedule { get; set; }
    }
}
