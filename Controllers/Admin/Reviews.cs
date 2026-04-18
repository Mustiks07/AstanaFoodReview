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
public class AdminReviewsController : Controller
{
    private readonly DataManager _data;
    private readonly UserManager<IdentityUser> _userManager;

    public AdminReviewsController(DataManager data, UserManager<IdentityUser> userManager)
    {
        _data = data;
        _userManager = userManager;
    }

    [Route("")]
    public async Task<IActionResult> Index()
    {
        var restaurants = (await _data.Restaurants.GetRestaurantsAsync()).ToList();
        var allReviews = new List<ReviewDTO>();
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
        await _data.OwnerResponses.SaveAsync(new OwnerResponse
        {
            ReviewId = id,
            AdminId  = user?.Id,
            Text     = text
        });
        return RedirectToAction(nameof(Index));
    }
}
