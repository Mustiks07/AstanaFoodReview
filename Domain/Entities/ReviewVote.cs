using AstanaFoodReviews.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace AstanaFoodReviews.Domain.Entities;

public class ReviewVote
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public IdentityUser? User { get; set; }
    public int ReviewId { get; set; }
    public Review? Review { get; set; }
    public ReviewVoteTypeEnum VoteType { get; set; }
}
