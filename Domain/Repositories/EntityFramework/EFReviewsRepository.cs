using AstanaFoodReviews.Domain.Entities;
using AstanaFoodReviews.Domain.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace AstanaFoodReviews.Domain.Repositories.EntityFramework;

public class EFReviewsRepository : IReviewsRepository
{
    private readonly AppDbContext _context;

    public EFReviewsRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Review>> GetReviewsByRestaurantAsync(int restaurantId)
    {
        return await _context.Reviews
            .Include(r => r.Author)
            .Include(r => r.Votes)
            .Include(r => r.OwnerResponses).ThenInclude(o => o.Admin)
            .Where(r => r.RestaurantId == restaurantId)
            .OrderByDescending(r => r.DateCreated)
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetReviewsByAuthorAsync(string authorId)
    {
        return await _context.Reviews
            .Include(r => r.Restaurant)
            .Include(r => r.OwnerResponses)
            .Where(r => r.AuthorId == authorId)
            .OrderByDescending(r => r.DateCreated)
            .ToListAsync();
    }

    public async Task<Review?> GetReviewByIdAsync(int id)
    {
        return await _context.Reviews
            .Include(r => r.Author)
            .Include(r => r.Restaurant)
            .Include(r => r.Votes)
            .Include(r => r.OwnerResponses).ThenInclude(o => o.Admin)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<ReviewVote?> GetVoteAsync(string userId, int reviewId)
    {
        return await _context.ReviewVotes
            .FirstOrDefaultAsync(v => v.UserId == userId && v.ReviewId == reviewId);
    }

    public async Task SaveReviewAsync(Review entity)
    {
        if (entity.Id == 0)
            _context.Reviews.Add(entity);
        else
            _context.Reviews.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteReviewAsync(int id)
    {
        var entity = await _context.Reviews.FindAsync(id);
        if (entity != null)
        {
            _context.Reviews.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task SaveVoteAsync(ReviewVote vote)
    {
        if (vote.Id == 0)
            _context.ReviewVotes.Add(vote);
        else
            _context.ReviewVotes.Update(vote);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteVoteAsync(ReviewVote vote)
    {
        _context.ReviewVotes.Remove(vote);
        await _context.SaveChangesAsync();
    }
}
