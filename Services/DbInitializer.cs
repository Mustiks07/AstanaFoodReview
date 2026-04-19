using Microsoft.AspNetCore.Identity;

namespace AstanaFoodReviews.Services;

public static class DbInitializer
{
    // If admin still has the default "admin" password, upgrade it automatically.
    public static async Task HardenAdminAsync(IServiceProvider services)
    {
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var admin = await userManager.FindByNameAsync("admin");
        if (admin == null) return;

        var stillDefault = await userManager.CheckPasswordAsync(admin, "admin");
        if (!stillDefault) return;

        await userManager.RemovePasswordAsync(admin);
        await userManager.AddPasswordAsync(admin, "Admin@Astana2026!");
    }
}
