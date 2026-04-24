using AstanaFoodReviews.Domain;
using AstanaFoodReviews.Domain.Entities;
using AstanaFoodReviews.Domain.Enums;
using AstanaFoodReviews.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AstanaFoodReviews.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("admin/restaurants")]
public class AdminRestaurantsController : Controller
{
    private readonly DataManager _data;

    public AdminRestaurantsController(DataManager data) => _data = data;

    [Route("")]
    public async Task<IActionResult> Index()
    {
        var restaurants = await _data.Restaurants.GetRestaurantsAsync();
        return View("~/Views/Admin/Restaurants/Index.cshtml", HelperDTO.TransformRestaurants(restaurants));
    }

    [HttpGet("create")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Districts = await _data.Districts.GetDistrictsAsync();
        ViewBag.Cuisines  = await _data.Cuisines.GetCuisinesAsync();
        return View("~/Views/Admin/Restaurants/Create.cshtml");
    }

    [HttpPost("create"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        string title, string? description, string? address,
        string? phone, string? website, string? imageUrl,
        int districtId, int cuisineId, PriceRangeEnum priceRange, bool isVerified)
    {
        var restaurant = new Restaurant
        {
            Title       = title,
            Description = description,
            Address     = address,
            Phone       = phone,
            Website     = website,
            ImageUrl    = imageUrl,
            DistrictId  = districtId,
            CuisineId   = cuisineId,
            PriceRange  = priceRange,
            IsVerified  = isVerified,
            DateCreated = DateTime.UtcNow
        };
        await _data.Restaurants.SaveRestaurantAsync(restaurant);
        return RedirectToAction(nameof(Index));
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

    [HttpPost("edit/{id:int}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id, string title, string? description, string? address,
        string? phone, string? website, string? imageUrl,
        int districtId, int cuisineId, PriceRangeEnum priceRange, bool isVerified)
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
        restaurant.PriceRange  = priceRange;
        restaurant.IsVerified  = isVerified;

        await _data.Restaurants.SaveRestaurantAsync(restaurant);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("delete/{id:int}"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _data.Restaurants.DeleteRestaurantAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
