using AstanaFoodReviews.Domain;
using AstanaFoodReviews.Domain.Entities;
using AstanaFoodReviews.Domain.Enums;
using AstanaFoodReviews.Infrastructure;
using AstanaFoodReviews.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AstanaFoodReviews.Controllers;

public class RestaurantsController : Controller
{
    private readonly DataManager _data;
    private readonly UserManager<IdentityUser> _userManager;

    public RestaurantsController(DataManager data, UserManager<IdentityUser> userManager)
    {
        _data = data;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(
        string? search,
        int? districtId,
        int? cuisineId,
        string? sort,
        PriceRangeEnum? priceRange)
    {
        var restaurants = await _data.Restaurants.GetRestaurantsAsync(districtId, cuisineId, search, sort, priceRange);
        var districts   = await _data.Districts.GetDistrictsAsync();
        var cuisines    = await _data.Cuisines.GetCuisinesAsync();

        ViewBag.Districts  = districts;
        ViewBag.Cuisines   = cuisines;
        ViewBag.Search     = search;
        ViewBag.DistrictId = districtId;
        ViewBag.CuisineId  = cuisineId;
        ViewBag.Sort       = sort;
        ViewBag.PriceRange = priceRange;

        return View(HelperDTO.TransformRestaurants(restaurants));
    }

    public async Task<IActionResult> Show(int id)
    {
        var restaurant = await _data.Restaurants.GetRestaurantByIdAsync(id);
        if (restaurant == null) return NotFound();

        var currentUser = await _userManager.GetUserAsync(User);
        var isAdmin = currentUser != null && await _userManager.IsInRoleAsync(currentUser, "Admin");

        Dictionary<int, ReviewVoteTypeEnum?> userVotes = [];
        if (currentUser != null)
        {
            foreach (var review in restaurant.Reviews)
            {
                var vote = await _data.Reviews.GetVoteAsync(currentUser.Id, review.Id);
                userVotes[review.Id] = vote?.VoteType;
            }
        }

        ViewBag.CurrentUser = currentUser;
        ViewBag.UserVotes   = userVotes;
        ViewBag.IsAdmin     = isAdmin;
        ViewBag.Reviews     = restaurant.Reviews.OrderByDescending(r => r.DateCreated)
                                .Select(HelperDTO.TransformReview).ToList();

        return View(HelperDTO.Transform(restaurant));
    }

    [Authorize]
    public async Task<IActionResult> New()
    {
        var vm = new CreateRestaurantViewModel
        {
            Districts = await _data.Districts.GetDistrictsAsync(),
            Cuisines  = await _data.Cuisines.GetCuisinesAsync()
        };
        return View(vm);
    }

    [HttpPost, Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> New(CreateRestaurantViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Districts = await _data.Districts.GetDistrictsAsync();
            vm.Cuisines  = await _data.Cuisines.GetCuisinesAsync();
            return View(vm);
        }

        var user = await _userManager.GetUserAsync(User);
        var restaurant = new Restaurant
        {
            Title       = vm.Title,
            Description = vm.Description,
            Address     = vm.Address,
            Phone       = vm.Phone,
            Website     = vm.Website,
            ImageUrl    = vm.ImageUrl,
            DistrictId  = vm.DistrictId,
            CuisineId   = vm.CuisineId,
            PriceRange  = vm.PriceRange,
            OwnerId     = user?.Id
        };

        await _data.Restaurants.SaveRestaurantAsync(restaurant);
        return RedirectToAction(nameof(Show), new { id = restaurant.Id });
    }

    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var restaurant = await _data.Restaurants.GetRestaurantByIdAsync(id);
        if (restaurant == null) return NotFound();

        var user = await _userManager.GetUserAsync(User);
        var isAdmin = user != null && await _userManager.IsInRoleAsync(user, "Admin");

        if (!isAdmin && restaurant.OwnerId != user?.Id)
            return Forbid();

        var vm = new CreateRestaurantViewModel
        {
            Title       = restaurant.Title,
            Description = restaurant.Description,
            Address     = restaurant.Address,
            Phone       = restaurant.Phone,
            Website     = restaurant.Website,
            ImageUrl    = restaurant.ImageUrl,
            DistrictId  = restaurant.DistrictId,
            CuisineId   = restaurant.CuisineId,
            PriceRange  = restaurant.PriceRange,
            Districts   = await _data.Districts.GetDistrictsAsync(),
            Cuisines    = await _data.Cuisines.GetCuisinesAsync()
        };
        ViewBag.RestaurantId = id;
        return View(vm);
    }

    [HttpPost, Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CreateRestaurantViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            vm.Districts = await _data.Districts.GetDistrictsAsync();
            vm.Cuisines  = await _data.Cuisines.GetCuisinesAsync();
            ViewBag.RestaurantId = id;
            return View(vm);
        }

        var restaurant = await _data.Restaurants.GetRestaurantByIdAsync(id);
        if (restaurant == null) return NotFound();

        var user = await _userManager.GetUserAsync(User);
        var isAdmin = user != null && await _userManager.IsInRoleAsync(user, "Admin");

        if (!isAdmin && restaurant.OwnerId != user?.Id)
            return Forbid();

        restaurant.Title       = vm.Title;
        restaurant.Description = vm.Description;
        restaurant.Address     = vm.Address;
        restaurant.Phone       = vm.Phone;
        restaurant.Website     = vm.Website;
        restaurant.ImageUrl    = vm.ImageUrl;
        restaurant.DistrictId  = vm.DistrictId;
        restaurant.CuisineId   = vm.CuisineId;
        restaurant.PriceRange  = vm.PriceRange;

        await _data.Restaurants.SaveRestaurantAsync(restaurant);
        return RedirectToAction(nameof(Show), new { id });
    }

    [HttpPost, Authorize, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var restaurant = await _data.Restaurants.GetRestaurantByIdAsync(id);
        if (restaurant == null) return NotFound();

        var user = await _userManager.GetUserAsync(User);
        var isAdmin = user != null && await _userManager.IsInRoleAsync(user, "Admin");

        if (!isAdmin && restaurant.OwnerId != user?.Id)
            return Forbid();

        await _data.Restaurants.DeleteRestaurantAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
