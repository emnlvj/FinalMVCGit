﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentPortal.Models
{
	public class Student
	{
		[Key]
		public int Id { get; set; }
        [Required]

        [Range(0, int.MaxValue, ErrorMessage = "Count must be a non-negative value.")]
        public int count { get; set; }
		
		
		public string FName { get; set; }

		public string LName { get; set; }
		public string MName { get; set; }

		[Range(10,50, ErrorMessage = "Enter valid age")]
		public string Status { get; set; }

		public string Course { get; set; }
		public string Remarks { get; set; }

        [Range(1, 4)]
        public int Year { get; set; }

       
    }
}
