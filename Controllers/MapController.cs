using AstanaFoodReviews.Domain;
using Microsoft.AspNetCore.Mvc;

namespace AstanaFoodReviews.Controllers;

public class MapController : Controller
{
    private readonly DataManager _data;

    public MapController(DataManager data) => _data = data;

    public async Task<IActionResult> Index()
    {
        var districts = await _data.Districts.GetDistrictsAsync();
        return View(districts);
    }
}
