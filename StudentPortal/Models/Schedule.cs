using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentPortal.Models
{
    public class Schedule
    {
        [Key]
        public int EdpCode { get; set; }

        public TimeOnly starttime { get; set; }
        public TimeOnly endtime { get; set; }
        public string section { get; set; }
        public int roomnum { get; set; }
        public string curryear { get; set; }
        public string description { get; set; }

        public string AMPM { get; set; }
        public string days { get; set; }

        public Subject Subject { get; set; }


    }
}
