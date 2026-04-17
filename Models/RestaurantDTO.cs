using AstanaFoodReviews.Domain.Enums;

namespace AstanaFoodReviews.Models;

public class RestaurantDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }
    public string? ImageUrl { get; set; }
    public string PriceRangeLabel { get; set; } = string.Empty;
    public PriceRangeEnum PriceRange { get; set; }
    public string DistrictName { get; set; } = string.Empty;
    public int DistrictId { get; set; }
    public string CuisineName { get; set; } = string.Empty;
    public int CuisineId { get; set; }
    public bool IsVerified { get; set; }
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public DateTime DateCreated { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}
