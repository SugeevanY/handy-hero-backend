using System.ComponentModel.DataAnnotations;

namespace HandyHero.Models
{
    public class Customer
    {
        internal string email;

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }

        public String? VerificationCode { get; set; }
    }
}
