using AstanaFoodReviews.Domain.Entities;
using AstanaFoodReviews.Domain.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;

namespace AstanaFoodReviews.Domain.Repositories.EntityFramework;

public class EFCuisinesRepository : ICuisinesRepository
{
    private readonly AppDbContext _context;

    public EFCuisinesRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Cuisine>> GetCuisinesAsync()
    {
        return await _context.Cuisines
            .Include(c => c.Restaurants)
            .OrderBy(c => c.Title)
            .ToListAsync();
    }

    public async Task<Cuisine?> GetCuisineByIdAsync(int id)
    {
        return await _context.Cuisines
            .Include(c => c.Restaurants)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task SaveCuisineAsync(Cuisine entity)
    {
        if (entity.Id == 0)
            _context.Cuisines.Add(entity);
        else
            _context.Cuisines.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCuisineAsync(int id)
    {
        var entity = await _context.Cuisines.FindAsync(id);
        if (entity != null)
        {
            _context.Cuisines.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
