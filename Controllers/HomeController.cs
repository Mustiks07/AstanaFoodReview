using AstanaFoodReviews.Domain;
using AstanaFoodReviews.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace AstanaFoodReviews.Controllers;

public class HomeController : Controller
{
    private readonly DataManager _data;

    public HomeController(DataManager data) => _data = data;

    public async Task<IActionResult> Index()
    {
        var restaurants = await _data.Restaurants.GetRestaurantsAsync();
        var list = restaurants.ToList();
        var top3 = list.OrderByDescending(r => r.AverageRating).Take(3).ToList();

        ViewBag.RestaurantCount = list.Count;
        ViewBag.ReviewCount = list.Sum(r => r.ReviewCount);
        ViewBag.AverageRating = list.Any(r => r.ReviewCount > 0)
            ? Math.Round(list.Where(r => r.ReviewCount > 0).Average(r => r.AverageRating), 1)
            : 0;
        ViewBag.Top3 = HelperDTO.TransformRestaurants(top3).ToList();
        ViewBag.Cuisines = await _data.Cuisines.GetCuisinesAsync();

        return View();
    }

    public IActionResult About() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View();
}
