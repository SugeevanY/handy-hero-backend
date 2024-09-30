using System.ComponentModel.DataAnnotations;

namespace HandyHero.Models
{
    public class Review
    {

            [Key]
            public int ReviewId { get; set; }

            [Required]
            public int ReviewerId { get; set; }

            [Required]
            public string Email { get; set; }

            [Required]
            public string ReviewText { get; set; }

            public DateTime TimeStamp { get; set; }

            public string RatingValue { get; set; }




    }
    }



