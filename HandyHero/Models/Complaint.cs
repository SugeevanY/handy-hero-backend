using System.ComponentModel.DataAnnotations;

namespace HandyHero.Models
{
    public class Complaint
    {
        [Key]
        public int ComplaintId { get; set; }

        [Required]
        public string Complainant { get; set; }

        [Required]
        public string Accused { get; set; }

        [Required]
        public string ComplaintMessage { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
