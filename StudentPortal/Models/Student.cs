using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentPortal.Models
{
	public class Student
	{
		[Key]
		public int Id { get; set; }
      
        
		public string FName { get; set; }

		public string LName { get; set; }
		public string MName { get; set; }
		public string Status { get; set; }

		public string Course { get; set; }
		public string Remarks { get; set; }

        
        public int Year { get; set; }

        public ICollection<StudentEnrollment> StudentEnrollment { get; set; }


    }
}
