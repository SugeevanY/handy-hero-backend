using HandyHero.Data;
using HandyHero.DTO;
using HandyHero.Models;
using HandyHero.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HandyHero.Services.Repository
{
    public class ReviewRepository : IReview
    {
        private  ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CreateReview(Review review)
        {
            try
            {
                _context.Review.Add(review);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public List<ReviewView> GetReviewByEmail(string email)
        {
            var reviews = _context.Review.Where(r => r.Email == email).ToList();
            var reviewViews = new List<ReviewView>();

            foreach (var review in reviews)
            {
                var reviewView = new ReviewView
                {
                   // ReviewId = review.ReviewId,
                    ReviewerId = review.ReviewId,
                    Email = review.Email,
                    ReviewText = review.ReviewText,

                };

                reviewViews.Add(reviewView);
            }

            return reviewViews;
        }
    }
}
