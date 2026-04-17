using AstanaFoodReviews.Domain.Entities;
using AstanaFoodReviews.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AstanaFoodReviews.Domain;

public class AppDbContext : IdentityDbContext
{
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<ReviewVote> ReviewVotes { get; set; }
    public DbSet<District> Districts { get; set; }
    public DbSet<Cuisine> Cuisines { get; set; }
    public DbSet<OwnerResponse> OwnerResponses { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ReviewVote>()
            .HasIndex(v => new { v.UserId, v.ReviewId })
            .IsUnique();

        var seedDate = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Seed roles
        var adminRoleId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890";
        var userRoleId  = "b2c3d4e5-f6a7-8901-bcde-f12345678901";
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = adminRoleId },
            new IdentityRole { Id = userRoleId,  Name = "User",  NormalizedName = "USER",  ConcurrencyStamp = userRoleId  }
        );

        // Seed admin user
        var adminUserId = "c3d4e5f6-a7b8-9012-cdef-123456789012";
        var hasher = new PasswordHasher<IdentityUser>();
        var adminUser = new IdentityUser
        {
            Id = adminUserId,
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@astanafood.kz",
            NormalizedEmail = "ADMIN@ASTANAFOOD.KZ",
            EmailConfirmed = true,
            SecurityStamp = adminUserId,
            ConcurrencyStamp = adminUserId
        };
        adminUser.PasswordHash = hasher.HashPassword(adminUser, "admin");
        builder.Entity<IdentityUser>().HasData(adminUser);
        builder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string> { UserId = adminUserId, RoleId = adminRoleId }
        );

        // Seed districts
        builder.Entity<District>().HasData(
            new District { Id = 1, Title = "Алматы",   DateCreated = seedDate },
            new District { Id = 2, Title = "Байқоңыр", DateCreated = seedDate },
            new District { Id = 3, Title = "Есіл",     DateCreated = seedDate },
            new District { Id = 4, Title = "Нұра",     DateCreated = seedDate },
            new District { Id = 5, Title = "Сарыарқа", DateCreated = seedDate },
            new District { Id = 6, Title = "Жетісу",   DateCreated = seedDate },
            new District { Id = 7, Title = "Алатау",   DateCreated = seedDate },
            new District { Id = 8, Title = "Байтерек", DateCreated = seedDate }
        );

        // Seed cuisines
        builder.Entity<Cuisine>().HasData(
            new Cuisine { Id = 1, Title = "Казахская",    DateCreated = seedDate },
            new Cuisine { Id = 2, Title = "Европейская",  DateCreated = seedDate },
            new Cuisine { Id = 3, Title = "Итальянская",  DateCreated = seedDate },
            new Cuisine { Id = 4, Title = "Японская",     DateCreated = seedDate },
            new Cuisine { Id = 5, Title = "Китайская",    DateCreated = seedDate },
            new Cuisine { Id = 6, Title = "Кавказская",   DateCreated = seedDate },
            new Cuisine { Id = 7, Title = "Фастфуд",      DateCreated = seedDate },
            new Cuisine { Id = 8, Title = "Кофейня",      DateCreated = seedDate }
        );
    }
}
