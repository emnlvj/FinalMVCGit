using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace StudentPortal.Models
{
    public class Subject
    {
        [Key]
        
        [StringLength(20, ErrorMessage = "Subject Code cannot be longer than 20 characters.")]
        public string SubjCode { get; set; }

        [Required]
       
        [StringLength(100, ErrorMessage = "Subject Code cannot be longer than 100 characters.")]
        
        public string Descript {  get; set; }

        [Range(0, 4,  ErrorMessage = "Enter valid units (0-4)")]
        public int Units { get; set; }
        [StringLength(20, ErrorMessage = "Subject Code cannot be longer than 20 characters.")]
        public string Offering { get; set; }
        [StringLength(10, ErrorMessage = "Subject Code cannot be longer than 10 characters.")]
        public string CatCourse { get; set; }

        
        [StringLength(9, ErrorMessage = "Subject Code cannot be longer than 9 characters.")]
        public string CurrYear { get; set; }
       

        public ICollection<Schedule> Schedule { get; set; } = new List<Schedule>();
        public ICollection<PreRequisite> PreRequisite { get; set; } = new List<PreRequisite>();
    }
}
