using System.ComponentModel.DataAnnotations.Schema;

namespace StudentPortal.Models
{
    public class StudentEnrollment
    {
        [ForeignKey("Student")]
        public int Id { get; set; }
        public Student Student { get; set; }

        [ForeignKey("Schedule")]
        public int SubEdpCode { get; set; }
        public Schedule Schedule { get; set; }

    }

    
}
