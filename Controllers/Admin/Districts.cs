using AstanaFoodReviews.Domain;
using AstanaFoodReviews.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AstanaFoodReviews.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("admin/districts")]
public class Districts : Controller
{
    private readonly DataManager _data;

    public Districts(DataManager data) => _data = data;

    [Route("")]
    public async Task<IActionResult> Index()
        => View("~/Views/Admin/Districts/Index.cshtml", await _data.Districts.GetDistrictsAsync());

    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        if (id == 0) return View("~/Views/Admin/Districts/Edit.cshtml", new District());
        var entity = await _data.Districts.GetDistrictByIdAsync(id);
        if (entity == null) return NotFound();
        return View("~/Views/Admin/Districts/Edit.cshtml", entity);
    }

    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, string title)
    {
        var entity = id == 0 ? new District() : await _data.Districts.GetDistrictByIdAsync(id) ?? new District();
        entity.Title = title;
        await _data.Districts.SaveDistrictAsync(entity);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost("delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _data.Districts.DeleteDistrictAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
