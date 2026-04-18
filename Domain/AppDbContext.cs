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

        // Roles
        var adminRoleId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890";
        var userRoleId  = "b2c3d4e5-f6a7-8901-bcde-f12345678901";
        builder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = adminRoleId },
            new IdentityRole { Id = userRoleId,  Name = "User",  NormalizedName = "USER",  ConcurrencyStamp = userRoleId  }
        );

        // Admin user
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

        // Districts
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

        // Cuisines
        builder.Entity<Cuisine>().HasData(
            new Cuisine { Id = 1, Title = "Казахская",   DateCreated = seedDate },
            new Cuisine { Id = 2, Title = "Европейская", DateCreated = seedDate },
            new Cuisine { Id = 3, Title = "Итальянская", DateCreated = seedDate },
            new Cuisine { Id = 4, Title = "Японская",    DateCreated = seedDate },
            new Cuisine { Id = 5, Title = "Китайская",   DateCreated = seedDate },
            new Cuisine { Id = 6, Title = "Кавказская",  DateCreated = seedDate },
            new Cuisine { Id = 7, Title = "Фастфуд",     DateCreated = seedDate },
            new Cuisine { Id = 8, Title = "Кофейня",     DateCreated = seedDate }
        );

        // Seed restaurants
        builder.Entity<Restaurant>().HasData(
            new Restaurant
            {
                Id = 1, Title = "Nomad Steak House",
                Description = "Лучшие стейки из казахстанской говядины в сердце Астаны. Уютная атмосфера и традиционные рецепты.",
                Address = "пр. Кабанбай батыра, 11, ЖК Expo",
                Phone = "+7 (717) 200-11-22",
                Website = "https://nomad.kz",
                PriceRange = PriceRangeEnum.Premium,
                IsVerified = true,
                DistrictId = 3, CuisineId = 1,
                DateCreated = seedDate
            },
            new Restaurant
            {
                Id = 2, Title = "Sushi Boom",
                Description = "Широкий выбор роллов и суши из свежих ингредиентов. Доставка и зал.",
                Address = "ул. Достык, 5, ТЦ Хан Шатыр",
                Phone = "+7 (717) 233-44-55",
                PriceRange = PriceRangeEnum.Medium,
                IsVerified = true,
                DistrictId = 3, CuisineId = 4,
                DateCreated = seedDate
            },
            new Restaurant
            {
                Id = 3, Title = "La Piazza",
                Description = "Итальянская пиццерия с дровяной печью. Паста, ризотто и живая музыка по пятницам.",
                Address = "пр. Туран, 24",
                Phone = "+7 (717) 255-66-77",
                PriceRange = PriceRangeEnum.Medium,
                IsVerified = true,
                DistrictId = 3, CuisineId = 3,
                DateCreated = seedDate
            },
            new Restaurant
            {
                Id = 4, Title = "Coffeemania",
                Description = "Авторский specialty-кофе, завтраки и десерты в современном интерьере.",
                Address = "ул. Сыганак, 14",
                Phone = "+7 (717) 277-88-99",
                PriceRange = PriceRangeEnum.Medium,
                IsVerified = true,
                DistrictId = 3, CuisineId = 8,
                DateCreated = seedDate
            },
            new Restaurant
            {
                Id = 5, Title = "Чинара",
                Description = "Кавказская кухня: шашлыки, хинкали, хачапури. Большие порции и семейная атмосфера.",
                Address = "ул. Бейбітшілік, 18",
                Phone = "+7 (717) 211-22-33",
                PriceRange = PriceRangeEnum.Medium,
                IsVerified = false,
                DistrictId = 8, CuisineId = 6,
                DateCreated = seedDate
            },
            new Restaurant
            {
                Id = 6, Title = "Dragon Palace",
                Description = "Аутентичная китайская кухня: dim sum, утка по-пекински и морепродукты.",
                Address = "пр. Республики, 33",
                Phone = "+7 (717) 244-55-66",
                PriceRange = PriceRangeEnum.Medium,
                IsVerified = false,
                DistrictId = 4, CuisineId = 5,
                DateCreated = seedDate
            },
            new Restaurant
            {
                Id = 7, Title = "Brasserie Française",
                Description = "Французская бистро-кухня: круассаны, стейк-фрит и бургундское вино.",
                Address = "пр. Мәңгілік Ел, 55, Expo City",
                Phone = "+7 (717) 299-00-11",
                PriceRange = PriceRangeEnum.Luxury,
                IsVerified = true,
                DistrictId = 3, CuisineId = 2,
                DateCreated = seedDate
            },
            new Restaurant
            {
                Id = 8, Title = "Burger House",
                Description = "Сочные бургеры из свежей говядины, картошка фри и молочные коктейли.",
                Address = "ул. Сарайшык, 7",
                Phone = "+7 (717) 222-33-44",
                PriceRange = PriceRangeEnum.Budget,
                IsVerified = false,
                DistrictId = 5, CuisineId = 7,
                DateCreated = seedDate
            }
        );
    }
}
