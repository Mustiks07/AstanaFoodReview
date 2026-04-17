using Microsoft.AspNetCore.Identity;

namespace AstanaFoodReviews.Domain.Entities;

public class OwnerResponse
{
    public int Id { get; set; }
    public int ReviewId { get; set; }
    public Review? Review { get; set; }
    public string? AdminId { get; set; }
    public IdentityUser? Admin { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}
