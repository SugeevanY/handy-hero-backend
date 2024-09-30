using System.ComponentModel.DataAnnotations;

namespace HandyHero.Models
{
    public class FieldWorker
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string District { get; set; }

        [Required]
        public string WorkType { get; set; }
        public DateTime DOB { get; set; }
        public string NIC { get; set; }
        public string ProfileImage { get; set; }

        public string[] Certificates { get; set; }

        
        public string[] ExperienceLetter { get; set; }
        [Required]
        public int Rating { get; set; }

        [Required]
        public string Status { get; set; } = "false";

        public int AcceptOrRejectBy { get; set; }

        public bool isHired {  get; set; }
        public string? VerificationCode { get; set; }
    }
}
