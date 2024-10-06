using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace StudentPortal.Models
{
    public class Subject
    {
        [Key]
       public string SubjCode { get; set; }

        [Required]
        public string Descript {  get; set; }
        public int Units { get; set; }
        public string Offering { get; set; }
        public string CatCourse { get; set; }
        public int CourseCode { get; set; }
        public string CurrYear { get; set; }

        public ICollection<PreRequisite> PreRequisite { get; set; } = new List<PreRequisite>();
       
    }
}
