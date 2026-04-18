using AstanaFoodReviews.Domain;
using AstanaFoodReviews.Domain.Entities;
using AstanaFoodReviews.Infrastructure;
using AstanaFoodReviews.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AstanaFoodReviews.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("admin/reviews")]
public class Reviews : Controller
{
    private readonly DataManager _data;
    private readonly UserManager<IdentityUser> _userManager;

    public Reviews(DataManager data, UserManager<IdentityUser> userManager)
    {
        _data = data;
        _userManager = userManager;
    }

    [Route("")]
    public async Task<IActionResult> Index()
    {
        // Collect all reviews from all restaurants
        var restaurants = (await _data.Restaurants.GetRestaurantsAsync()).ToList();
        var allReviews = new List<Models.ReviewDTO>();
        foreach (var r in restaurants)
        {
            var reviews = await _data.Reviews.GetReviewsByRestaurantAsync(r.Id);
            allReviews.AddRange(reviews.Select(HelperDTO.TransformReview));
        }
        return View("~/Views/Admin/Reviews/Index.cshtml", allReviews.OrderByDescending(r => r.DateCreated));
    }

    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _data.Reviews.DeleteReviewAsync(id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("respond/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Respond(int id, string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return RedirectToAction(nameof(Index));

        var user = await _userManager.GetUserAsync(User);
        var response = new OwnerResponse
        {
            ReviewId = id,
            AdminId  = user?.Id,
            Text     = text
        };

        await _data.OwnerResponses.SaveAsync(response);
        return RedirectToAction(nameof(Index));
    }
}
