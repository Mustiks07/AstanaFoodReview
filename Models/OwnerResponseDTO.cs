namespace AstanaFoodReviews.Models;

public class OwnerResponseDTO
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public string? AdminName { get; set; }
    public DateTime DateCreated { get; set; }
}
