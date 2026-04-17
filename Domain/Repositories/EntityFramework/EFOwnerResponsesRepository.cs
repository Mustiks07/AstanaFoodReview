using AstanaFoodReviews.Domain.Entities;
using AstanaFoodReviews.Domain.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace AstanaFoodReviews.Domain.Repositories.EntityFramework;

public class EFOwnerResponsesRepository : IOwnerResponsesRepository
{
    private readonly AppDbContext _context;

    public EFOwnerResponsesRepository(AppDbContext context) => _context = context;

    public async Task<OwnerResponse?> GetByIdAsync(int id)
    {
        return await _context.OwnerResponses
            .Include(o => o.Admin)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task SaveAsync(OwnerResponse entity)
    {
        if (entity.Id == 0)
            _context.OwnerResponses.Add(entity);
        else
            _context.OwnerResponses.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.OwnerResponses.FindAsync(id);
        if (entity != null)
        {
            _context.OwnerResponses.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
