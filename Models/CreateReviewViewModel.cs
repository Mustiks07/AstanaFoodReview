using System.ComponentModel.DataAnnotations;

namespace AstanaFoodReviews.Models;

public class CreateReviewViewModel
{
    public int RestaurantId { get; set; }
    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    public string? Text { get; set; }
    [Range(1, 5)]
    public int Rating { get; set; } = 5;
    [Range(1, 5)]
    public int FoodRating { get; set; } = 5;
    [Range(1, 5)]
    public int ServiceRating { get; set; } = 5;
    [Range(1, 5)]
    public int PriceRating { get; set; } = 5;
}
