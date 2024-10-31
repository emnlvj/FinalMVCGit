using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentPortal.Models
{
    public class Schedule
    {
        [Key]
        
        public int SubEdpCode { get; set; }

        [Required]
        public TimeOnly starttime { get; set; }
        [Required]
        public TimeOnly endtime { get; set; }
        [Required]
        public string section { get; set; }
        [Required]
        public int roomnum { get; set; }
        [Required]
        public string curryear { get; set; }
        [Required]
        public string description { get; set; }
        [Required]
        public string AMPM { get; set; }
        [Required]
        public string days { get; set; }
        [Required]
        [ForeignKey("Subject")]

        public string SubjCode { get; set; }
        public Subject Subject { get; set; }
        


    }
}
