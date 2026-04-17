using AstanaFoodReviews.Domain;
using AstanaFoodReviews.Domain.Entities;
using AstanaFoodReviews.Domain.Enums;
using AstanaFoodReviews.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AstanaFoodReviews.Controllers;

[Authorize]
public class ReviewsController : Controller
{
    private readonly DataManager _data;
    private readonly UserManager<IdentityUser> _userManager;

    public ReviewsController(DataManager data, UserManager<IdentityUser> userManager)
    {
        _data = data;
        _userManager = userManager;
    }

    [HttpGet("/reviews/new/{restaurantId:int}")]
    public async Task<IActionResult> New(int restaurantId)
    {
        var restaurant = await _data.Restaurants.GetRestaurantByIdAsync(restaurantId);
        if (restaurant == null) return NotFound();

        ViewBag.RestaurantName = restaurant.Title;
        return View(new CreateReviewViewModel { RestaurantId = restaurantId });
    }

    [HttpPost("/reviews/new")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> New(CreateReviewViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            var rest = await _data.Restaurants.GetRestaurantByIdAsync(vm.RestaurantId);
            ViewBag.RestaurantName = rest?.Title ?? "";
            return View(vm);
        }

        var user = await _userManager.GetUserAsync(User);
        var review = new Review
        {
            Title          = vm.Title,
            Text           = vm.Text,
            Rating         = vm.Rating,
            FoodRating     = vm.FoodRating,
            ServiceRating  = vm.ServiceRating,
            PriceRating    = vm.PriceRating,
            RestaurantId   = vm.RestaurantId,
            AuthorId       = user?.Id
        };

        await _data.Reviews.SaveReviewAsync(review);
        return RedirectToAction("Show", "Restaurants", new { id = vm.RestaurantId });
    }

    [HttpPost("/reviews/delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var review = await _data.Reviews.GetReviewByIdAsync(id);
        if (review == null) return NotFound();

        var user = await _userManager.GetUserAsync(User);
        var isAdmin = user != null && await _userManager.IsInRoleAsync(user, "Admin");

        if (!isAdmin && review.AuthorId != user?.Id)
            return Forbid();

        var restaurantId = review.RestaurantId;
        await _data.Reviews.DeleteReviewAsync(id);
        return RedirectToAction("Show", "Restaurants", new { id = restaurantId });
    }

    [HttpPost("/reviews/vote")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Vote(int reviewId, ReviewVoteTypeEnum voteType)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Forbid();

        var review = await _data.Reviews.GetReviewByIdAsync(reviewId);
        if (review == null) return NotFound();

        var existing = await _data.Reviews.GetVoteAsync(user.Id, reviewId);

        if (existing != null && existing.VoteType == voteType)
        {
            await _data.Reviews.DeleteVoteAsync(existing);
        }
        else if (existing != null)
        {
            existing.VoteType = voteType;
            await _data.Reviews.SaveVoteAsync(existing);
        }
        else
        {
            await _data.Reviews.SaveVoteAsync(new ReviewVote
            {
                UserId   = user.Id,
                ReviewId = reviewId,
                VoteType = voteType
            });
        }

        // Recalculate counts
        var updatedReview = await _data.Reviews.GetReviewByIdAsync(reviewId);
        if (updatedReview != null)
        {
            updatedReview.UsefulCount    = updatedReview.Votes.Count(v => v.VoteType == ReviewVoteTypeEnum.Useful);
            updatedReview.NotUsefulCount = updatedReview.Votes.Count(v => v.VoteType == ReviewVoteTypeEnum.NotUseful);
            await _data.Reviews.SaveReviewAsync(updatedReview);
        }

        return RedirectToAction("Show", "Restaurants", new { id = review.RestaurantId });
    }
}
