namespace AstanaFoodReviews.Domain.Entities;

public class District : EntityBase
{
    public ICollection<Restaurant> Restaurants { get; set; } = [];
}
