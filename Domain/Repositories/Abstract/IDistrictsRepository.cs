using AstanaFoodReviews.Domain.Entities;

namespace AstanaFoodReviews.Domain.Repositories.Abstract;

public interface IDistrictsRepository
{
    Task<IEnumerable<District>> GetDistrictsAsync();
    Task<District?> GetDistrictByIdAsync(int id);
    Task SaveDistrictAsync(District entity);
    Task DeleteDistrictAsync(int id);
}
