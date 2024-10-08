using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentPortal.Models
{
    public class PreRequisite
    {
        [Key]
        
        public string PreSubjCode { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Subject Code cannot be longer than 100 characters.")]
        public string PreDescript { get; set;}
        [Required]
        [Range(0, 3)] 
        public int PreUnits { get; set;}
        [Required]
        [StringLength(50, ErrorMessage = "Subject Code cannot be longer than 10 characters.")]
        [ForeignKey("Subject")]

        public string SubjCode { get; set; }
        public Subject Subject { get; set; }

    }
}
