using AstanaFoodReviews.Domain.Entities;
using AstanaFoodReviews.Domain.Enums;
using AstanaFoodReviews.Models;

namespace AstanaFoodReviews.Infrastructure;

public static class HelperDTO
{
    public static RestaurantDTO Transform(Restaurant r) => new()
    {
        Id = r.Id,
        Title = r.Title,
        Description = r.Description,
        Address = r.Address,
        Phone = r.Phone,
        Website = r.Website,
        ImageUrl = r.ImageUrl,
        PriceRange = r.PriceRange,
        PriceRangeLabel = r.PriceRange switch
        {
            PriceRangeEnum.Budget  => "до 2000 ₸",
            PriceRangeEnum.Medium  => "2000–5000 ₸",
            PriceRangeEnum.Premium => "5000–15000 ₸",
            PriceRangeEnum.Luxury  => "от 15000 ₸",
            _ => ""
        },
        DistrictName = r.District?.Title ?? "",
        DistrictId = r.DistrictId,
        CuisineName = r.Cuisine?.Title ?? "",
        CuisineId = r.CuisineId,
        IsVerified = r.IsVerified,
        AverageRating = r.Reviews.Any() ? Math.Round(r.Reviews.Average(x => x.Rating), 1) : 0,
        ReviewCount = r.Reviews.Count,
        DateCreated = r.DateCreated,
        Latitude = r.Latitude,
        Longitude = r.Longitude,
    };

    public static IEnumerable<RestaurantDTO> TransformRestaurants(IEnumerable<Restaurant> list)
        => list.Select(Transform);

    public static ReviewDTO TransformReview(Review r) => new()
    {
        Id = r.Id,
        Title = r.Title,
        Text = r.Text,
        Rating = r.Rating,
        FoodRating = r.FoodRating,
        ServiceRating = r.ServiceRating,
        PriceRating = r.PriceRating,
        RestaurantId = r.RestaurantId,
        RestaurantName = r.Restaurant?.Title ?? "",
        AuthorId = r.AuthorId,
        AuthorName = r.Author?.UserName,
        UsefulCount = r.UsefulCount,
        NotUsefulCount = r.NotUsefulCount,
        DateCreated = r.DateCreated,
        OwnerResponses = r.OwnerResponses?.Select(o => new OwnerResponseDTO
        {
            Id = o.Id,
            Text = o.Text,
            AdminName = o.Admin?.UserName,
            DateCreated = o.DateCreated
        }).ToList() ?? []
    };

    public static string StarsHtml(double rating)
    {
        var full = (int)Math.Round(rating);
        return string.Concat(
            Enumerable.Range(1, 5).Select(i =>
                $"<span style='color:{(i <= full ? "#F97316" : "rgba(255,255,255,0.15)")}'>★</span>")
        );
    }
}
