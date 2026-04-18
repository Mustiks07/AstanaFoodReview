using AstanaFoodReviews.Domain;
using AstanaFoodReviews.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AstanaFoodReviews.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("admin/restaurants")]
public class Restaurants : Controller
{
    private readonly DataManager _data;

    public Restaurants(DataManager data) => _data = data;

    [Route("")]
    public async Task<IActionResult> Index()
    {
        var restaurants = await _data.Restaurants.GetRestaurantsAsync();
        return View("~/Views/Admin/Restaurants/Index.cshtml", HelperDTO.TransformRestaurants(restaurants));
    }

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var restaurant = await _data.Restaurants.GetRestaurantByIdAsync(id);
        if (restaurant == null) return NotFound();

        ViewBag.Districts = await _data.Districts.GetDistrictsAsync();
        ViewBag.Cuisines  = await _data.Cuisines.GetCuisinesAsync();
        return View("~/Views/Admin/Restaurants/Edit.cshtml", restaurant);
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        string title,
        string? description,
        string? address,
        string? phone,
        string? website,
        string? imageUrl,
        int districtId,
        int cuisineId,
        bool isVerified)
    {
        var restaurant = await _data.Restaurants.GetRestaurantByIdAsync(id);
        if (restaurant == null) return NotFound();

        restaurant.Title       = title;
        restaurant.Description = description;
        restaurant.Address     = address;
        restaurant.Phone       = phone;
        restaurant.Website     = website;
        restaurant.ImageUrl    = imageUrl;
        restaurant.DistrictId  = districtId;
        restaurant.CuisineId   = cuisineId;
        restaurant.IsVerified  = isVerified;

        await _data.Restaurants.SaveRestaurantAsync(restaurant);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _data.Restaurants.DeleteRestaurantAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
