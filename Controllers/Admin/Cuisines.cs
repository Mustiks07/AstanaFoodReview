using AstanaFoodReviews.Domain;
using AstanaFoodReviews.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AstanaFoodReviews.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("admin/cuisines")]
public class AdminCuisinesController : Controller
{
    private readonly DataManager _data;

    public AdminCuisinesController(DataManager data) => _data = data;

    [Route("")]
    public async Task<IActionResult> Index()
        => View("~/Views/Admin/Cuisines/Index.cshtml", await _data.Cuisines.GetCuisinesAsync());

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        if (id == 0) return View("~/Views/Admin/Cuisines/Edit.cshtml", new Cuisine());
        var entity = await _data.Cuisines.GetCuisineByIdAsync(id);
        if (entity == null) return NotFound();
        return View("~/Views/Admin/Cuisines/Edit.cshtml", entity);
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, string title)
    {
        var entity = id == 0 ? new Cuisine() : await _data.Cuisines.GetCuisineByIdAsync(id) ?? new Cuisine();
        entity.Title = title;
        await _data.Cuisines.SaveCuisineAsync(entity);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _data.Cuisines.DeleteCuisineAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
