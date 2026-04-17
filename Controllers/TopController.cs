using AstanaFoodReviews.Domain;
using AstanaFoodReviews.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace AstanaFoodReviews.Controllers;

public class TopController : Controller
{
    private readonly DataManager _data;

    public TopController(DataManager data) => _data = data;

    public async Task<IActionResult> Index()
    {
        var top = await _data.Restaurants.GetTopRestaurantsAsync(20);
        return View(HelperDTO.TransformRestaurants(top));
    }
}
