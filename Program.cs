using AstanaFoodReviews.Domain;
using AstanaFoodReviews.Domain.Repositories.Abstract;
using AstanaFoodReviews.Domain.Repositories.EntityFramework;
using AstanaFoodReviews.Infrastructure;
using AstanaFoodReviews.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

if (string.IsNullOrWhiteSpace(connectionString) || connectionString == "YOUR_CONNECTION_STRING")
    throw new InvalidOperationException(
        "DefaultConnection is empty or placeholder. " +
        "Fill in appsettings.json or appsettings.Development.json with a valid SQL Server connection string.");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.ConfigureWarnings(w =>
        w.Ignore(RelationalEventId.PendingModelChangesWarning));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath        = "/account/login";
    options.AccessDeniedPath = "/account/accessdenied";
});

builder.Services.AddScoped<IRestaurantsRepository, EFRestaurantsRepository>();
builder.Services.AddScoped<IReviewsRepository, EFReviewsRepository>();
builder.Services.AddScoped<IDistrictsRepository, EFDistrictsRepository>();
builder.Services.AddScoped<ICuisinesRepository, EFCuisinesRepository>();
builder.Services.AddScoped<IOwnerResponsesRepository, EFOwnerResponsesRepository>();
builder.Services.AddScoped<DataManager>();

builder.Services.Configure<AppConfig>(builder.Configuration.GetSection("Project"));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<LangService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
