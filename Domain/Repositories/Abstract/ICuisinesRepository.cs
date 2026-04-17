using AstanaFoodReviews.Domain.Entities;

namespace AstanaFoodReviews.Domain.Repositories.Abstract;

public interface ICuisinesRepository
{
    Task<IEnumerable<Cuisine>> GetCuisinesAsync();
    Task<Cuisine?> GetCuisineByIdAsync(int id);
    Task SaveCuisineAsync(Cuisine entity);
    Task DeleteCuisineAsync(int id);
}
