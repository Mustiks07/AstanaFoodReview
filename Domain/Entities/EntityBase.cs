using System.ComponentModel.DataAnnotations;

namespace AstanaFoodReviews.Domain.Entities;

public abstract class EntityBase
{
    public int Id { get; set; }
    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}
