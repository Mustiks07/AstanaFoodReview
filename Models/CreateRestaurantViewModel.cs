using System.ComponentModel.DataAnnotations;
using AstanaFoodReviews.Domain.Entities;
using AstanaFoodReviews.Domain.Enums;

namespace AstanaFoodReviews.Models;

public class CreateRestaurantViewModel
{
    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    [Required]
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }
    public string? ImageUrl { get; set; }
    [Required]
    public int DistrictId { get; set; }
    [Required]
    public int CuisineId { get; set; }
    public PriceRangeEnum PriceRange { get; set; } = PriceRangeEnum.Medium;
    public IEnumerable<District> Districts { get; set; } = [];
    public IEnumerable<Cuisine> Cuisines { get; set; } = [];
}
