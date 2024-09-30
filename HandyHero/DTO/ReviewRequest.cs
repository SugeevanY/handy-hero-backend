namespace HandyHero.DTO
{
    public class ReviewRequest
    {
        public int ReviewerId { get; set; }
        public string Email { get; set; }
        public string Review { get; set; }
        public string RatingValue { get; set; }
    }
}
