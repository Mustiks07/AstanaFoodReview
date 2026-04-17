using AstanaFoodReviews.Domain;
using AstanaFoodReviews.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AstanaFoodReviews.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly DataManager _data;
    private readonly UserManager<IdentityUser> _userManager;

    public ProfileController(DataManager data, UserManager<IdentityUser> userManager)
    {
        _data = data;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");

        var reviews = await _data.Reviews.GetReviewsByAuthorAsync(user.Id);
        return View(reviews.Select(HelperDTO.TransformReview));
    }
}
