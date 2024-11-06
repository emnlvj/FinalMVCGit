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

        public HashSet<string> GetDaysAsSet()
        {
            var daysSet = new HashSet<string>();
            int i = 0;

            while (i < days.Length)
            {
                // Check for two-character day "TH"
                if (i < days.Length - 1 && days.Substring(i, 2) == "TH")
                {
                    daysSet.Add("TH");
                    i += 2; // Move by 2 characters
                }
                else
                {
                    // Single-character days: M, T, W, F, S
                    daysSet.Add(days[i].ToString());
                    i += 1; // Move by 1 character
                }
            }

            return daysSet;
        }



    }
}
