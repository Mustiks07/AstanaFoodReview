using Microsoft.AspNetCore.Identity;

namespace AstanaFoodReviews.Domain.Entities;

public class Review : EntityBase
{
    public string? Text { get; set; }
    public int Rating { get; set; }
    public int FoodRating { get; set; }
    public int ServiceRating { get; set; }
    public int PriceRating { get; set; }

    public int RestaurantId { get; set; }
    public Restaurant? Restaurant { get; set; }

    public string? AuthorId { get; set; }
    public IdentityUser? Author { get; set; }

    public int UsefulCount { get; set; } = 0;
    public int NotUsefulCount { get; set; } = 0;

    public ICollection<ReviewVote> Votes { get; set; } = [];
    public ICollection<OwnerResponse> OwnerResponses { get; set; } = [];
}
