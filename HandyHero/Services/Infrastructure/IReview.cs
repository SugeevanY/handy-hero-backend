using HandyHero.DTO;
using HandyHero.Models;

namespace HandyHero.Services.Infrastructure
{
    public interface IReview
    {
       public List<ReviewView> GetReviewByEmail(string email);
       public bool CreateReview(Review review);

    }
}
