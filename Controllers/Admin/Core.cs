using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AstanaFoodReviews.Controllers.Admin;

[Authorize(Roles = "Admin")]
[Route("admin")]
public class Core : Controller
{
    [Route("")]
    [Route("index")]
    public IActionResult Index() => View("~/Views/Admin/Core/Index.cshtml");
}
