namespace AstanaFoodReviews.Domain.Entities;

public class Cuisine : EntityBase
{
    public ICollection<Restaurant> Restaurants { get; set; } = [];
}
