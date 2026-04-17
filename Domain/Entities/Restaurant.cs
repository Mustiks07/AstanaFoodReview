using System.ComponentModel.DataAnnotations.Schema;
using AstanaFoodReviews.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace AstanaFoodReviews.Domain.Entities;

public class Restaurant : EntityBase
{
    public string? Description { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Website { get; set; }
    public string? ImageUrl { get; set; }
    public PriceRangeEnum PriceRange { get; set; } = PriceRangeEnum.Medium;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public bool IsVerified { get; set; } = false;

    public int DistrictId { get; set; }
    public District? District { get; set; }

    public int CuisineId { get; set; }
    public Cuisine? Cuisine { get; set; }

    public string? OwnerId { get; set; }
    public IdentityUser? Owner { get; set; }

    public ICollection<Review> Reviews { get; set; } = [];

    [NotMapped]
    public double AverageRating => Reviews.Any() ? Reviews.Average(r => r.Rating) : 0;
    [NotMapped]
    public int ReviewCount => Reviews.Count;
}
