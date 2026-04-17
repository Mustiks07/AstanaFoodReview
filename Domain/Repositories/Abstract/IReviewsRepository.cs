using AstanaFoodReviews.Domain.Entities;

namespace AstanaFoodReviews.Domain.Repositories.Abstract;

public interface IReviewsRepository
{
    Task<IEnumerable<Review>> GetReviewsByRestaurantAsync(int restaurantId);
    Task<IEnumerable<Review>> GetReviewsByAuthorAsync(string authorId);
    Task<Review?> GetReviewByIdAsync(int id);
    Task<ReviewVote?> GetVoteAsync(string userId, int reviewId);
    Task SaveReviewAsync(Review entity);
    Task DeleteReviewAsync(int id);
    Task SaveVoteAsync(ReviewVote vote);
    Task DeleteVoteAsync(ReviewVote vote);
}
