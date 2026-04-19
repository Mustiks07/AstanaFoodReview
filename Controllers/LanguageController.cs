using Microsoft.AspNetCore.Mvc;

namespace AstanaFoodReviews.Controllers;

public class LanguageController : Controller
{
    [HttpGet("/lang/{code}")]
    public IActionResult Set(string code, string? returnUrl)
    {
        if (code is "kz" or "ru")
        {
            Response.Cookies.Append("lang", code, new CookieOptions
            {
                Expires  = DateTimeOffset.UtcNow.AddYears(1),
                IsEssential = true,
                SameSite = SameSiteMode.Lax
            });
        }
        return Redirect(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);
    }
}
