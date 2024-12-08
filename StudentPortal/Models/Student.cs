using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentPortal.Models
{
	public class Student
	{
		[Key]
		public int Id { get; set; }
        [Required]
        
		public string FName { get; set; }

		public string LName { get; set; }
		public string MName { get; set; }
		public string Status { get; set; }

		public string Course { get; set; }
		public string Remarks { get; set; }

        [Range(1, 5)]
        public int Year { get; set; }

       
    }
}
