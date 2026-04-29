using AstanaFoodReviews.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AstanaFoodReviews.Controllers;

public class MapController : Controller
{
    private readonly DataManager _data;
    private readonly UserManager<IdentityUser> _userManager;

    public MapController(DataManager data, UserManager<IdentityUser> userManager)
    {
        _data        = data;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var districts   = await _data.Districts.GetDistrictsAsync();
        var restaurants = await _data.Restaurants.GetRestaurantsAsync();

        var currentUser = await _userManager.GetUserAsync(User);

        // IDs ресторанов, на которые текущий пользователь уже оставил отзыв
        HashSet<int> reviewedIds = [];
        if (currentUser != null)
        {
            reviewedIds = restaurants
                .Where(r => r.Reviews.Any(rv => rv.AuthorId == currentUser.Id))
                .Select(r => r.Id)
                .ToHashSet();
        }

        ViewBag.Restaurants = restaurants;
        ViewBag.ReviewedIds = reviewedIds;

        return View(districts);
    }
}
