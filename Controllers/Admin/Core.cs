using AstanaFoodReviews.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AstanaFoodReviews.Controllers.Admin;

[Route("admin")]
public class AdminCoreController : Controller
{
    private readonly SignInManager<IdentityUser> _signIn;
    private readonly UserManager<IdentityUser>   _users;

    public AdminCoreController(SignInManager<IdentityUser> signIn, UserManager<IdentityUser> users)
    {
        _signIn = signIn;
        _users  = users;
    }

    // ── Admin panel dashboard ──────────────────────────────
    [Authorize(Roles = "Admin")]
    [Route("")]
    [Route("index")]
    public IActionResult Index() => View("~/Views/Admin/Core/Index.cshtml");

    // ── Separate admin login ───────────────────────────────
    [HttpGet("/admin/login")]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true && User.IsInRole("Admin"))
            return Redirect("/admin");
        return View("~/Views/Admin/Core/Login.cshtml", new LoginViewModel { ReturnUrl = "/admin" });
    }

    [HttpPost("/admin/login"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel vm)
    {
        if (!ModelState.IsValid)
            return View("~/Views/Admin/Core/Login.cshtml", vm);

        var result = await _signIn.PasswordSignInAsync(vm.UserName, vm.Password, false, false);
        if (result.Succeeded)
        {
            var user = await _users.FindByNameAsync(vm.UserName);
            if (user != null && await _users.IsInRoleAsync(user, "Admin"))
                return Redirect("/admin");

            // Logged in but not admin — sign out and reject
            await _signIn.SignOutAsync();
            ModelState.AddModelError("", "Доступ только для администраторов.");
            return View("~/Views/Admin/Core/Login.cshtml", vm);
        }

        ModelState.AddModelError("", "Неверный логин или пароль.");
        return View("~/Views/Admin/Core/Login.cshtml", vm);
    }

    [HttpPost("/admin/logout"), ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signIn.SignOutAsync();
        return Redirect("/admin/login");
    }
}
