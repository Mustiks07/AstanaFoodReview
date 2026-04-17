using AstanaFoodReviews.Domain.Repositories.Abstract;

namespace AstanaFoodReviews.Domain;

public class DataManager
{
    public IRestaurantsRepository Restaurants { get; }
    public IReviewsRepository Reviews { get; }
    public IDistrictsRepository Districts { get; }
    public ICuisinesRepository Cuisines { get; }
    public IOwnerResponsesRepository OwnerResponses { get; }

    public DataManager(
        IRestaurantsRepository restaurants,
        IReviewsRepository reviews,
        IDistrictsRepository districts,
        ICuisinesRepository cuisines,
        IOwnerResponsesRepository ownerResponses)
    {
        Restaurants = restaurants;
        Reviews = reviews;
        Districts = districts;
        Cuisines = cuisines;
        OwnerResponses = ownerResponses;
    }
}
