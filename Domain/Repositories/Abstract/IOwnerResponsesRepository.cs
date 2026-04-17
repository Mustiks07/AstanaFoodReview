using AstanaFoodReviews.Domain.Entities;

namespace AstanaFoodReviews.Domain.Repositories.Abstract;

public interface IOwnerResponsesRepository
{
    Task<OwnerResponse?> GetByIdAsync(int id);
    Task SaveAsync(OwnerResponse entity);
    Task DeleteAsync(int id);
}
