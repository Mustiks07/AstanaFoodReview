using AstanaFoodReviews.Domain.Entities;
using AstanaFoodReviews.Domain.Enums;
using AstanaFoodReviews.Domain.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace AstanaFoodReviews.Domain.Repositories.EntityFramework;

public class EFRestaurantsRepository : IRestaurantsRepository
{
    private readonly AppDbContext _context;

    public EFRestaurantsRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Restaurant>> GetRestaurantsAsync(
        int? districtId = null,
        int? cuisineId = null,
        string? search = null,
        string? sort = null,
        PriceRangeEnum? priceRange = null)
    {
        var query = _context.Restaurants
            .Include(r => r.District)
            .Include(r => r.Cuisine)
            .Include(r => r.Reviews)
            .AsQueryable();

        if (districtId.HasValue)
            query = query.Where(r => r.DistrictId == districtId.Value);

        if (cuisineId.HasValue)
            query = query.Where(r => r.CuisineId == cuisineId.Value);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(r =>
                r.Title.Contains(search) ||
                (r.Description != null && r.Description.Contains(search)) ||
                (r.Address != null && r.Address.Contains(search)));

        if (priceRange.HasValue)
            query = query.Where(r => r.PriceRange == priceRange.Value);

        var list = await query.ToListAsync();

        return sort switch
        {
            "rating"  => list.OrderByDescending(r => r.AverageRating).Take(100),
            "reviews" => list.OrderByDescending(r => r.ReviewCount).Take(100),
            _         => list.OrderByDescending(r => r.DateCreated).Take(100)
        };
    }

    public async Task<IEnumerable<Restaurant>> GetTopRestaurantsAsync(int count = 10)
    {
        var list = await _context.Restaurants
            .Include(r => r.District)
            .Include(r => r.Cuisine)
            .Include(r => r.Reviews)
            .ToListAsync();

        return list.OrderByDescending(r => r.AverageRating).Take(count);
    }

    public async Task<Restaurant?> GetRestaurantByIdAsync(int id)
    {
        return await _context.Restaurants
            .Include(r => r.District)
            .Include(r => r.Cuisine)
            .Include(r => r.Reviews)
                .ThenInclude(rv => rv.Author)
            .Include(r => r.Reviews)
                .ThenInclude(rv => rv.OwnerResponses)
                    .ThenInclude(o => o.Admin)
            .Include(r => r.Reviews)
                .ThenInclude(rv => rv.Votes)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task SaveRestaurantAsync(Restaurant entity)
    {
        if (entity.Id == 0)
            _context.Restaurants.Add(entity);
        else
            _context.Restaurants.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteRestaurantAsync(int id)
    {
        var entity = await _context.Restaurants.FindAsync(id);
        if (entity != null)
        {
            _context.Restaurants.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
