using AstanaFoodReviews.Domain.Entities;
using AstanaFoodReviews.Domain.Enums;

namespace AstanaFoodReviews.Domain.Repositories.Abstract;

public interface IRestaurantsRepository
{
    Task<IEnumerable<Restaurant>> GetRestaurantsAsync(
        int? districtId = null,
        int? cuisineId = null,
        string? search = null,
        string? sort = null,
        PriceRangeEnum? priceRange = null);
    Task<IEnumerable<Restaurant>> GetTopRestaurantsAsync(int count = 10);
    Task<Restaurant?> GetRestaurantByIdAsync(int id);
    Task SaveRestaurantAsync(Restaurant entity);
    Task DeleteRestaurantAsync(int id);
}
