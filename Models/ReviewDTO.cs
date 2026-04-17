namespace AstanaFoodReviews.Models;

public class ReviewDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Text { get; set; }
    public int Rating { get; set; }
    public int FoodRating { get; set; }
    public int ServiceRating { get; set; }
    public int PriceRating { get; set; }
    public int RestaurantId { get; set; }
    public string RestaurantName { get; set; } = string.Empty;
    public string? AuthorId { get; set; }
    public string? AuthorName { get; set; }
    public int UsefulCount { get; set; }
    public int NotUsefulCount { get; set; }
    public DateTime DateCreated { get; set; }
    public List<OwnerResponseDTO> OwnerResponses { get; set; } = [];
}
