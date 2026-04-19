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

        // Restaurants (20 штук)
        builder.Entity<Restaurant>().HasData(
            new Restaurant
            {
                Id = 1, Title = "Nomad Steak House",
                Description = "Лучшие стейки из казахстанской говядины. Уютная атмосфера и традиционные рецепты кочевников.",
                Address = "пр. Кабанбай батыра, 11", Phone = "+7 (717) 200-11-22",
                ImageUrl = "https://images.unsplash.com/photo-1544025162-d76538369348?w=600&fit=crop",
                PriceRange = PriceRangeEnum.Premium, IsVerified = true,
                DistrictId = 3, CuisineId = 1, DateCreated = seedDate
            },
            new Restaurant
            {
                Id = 2, Title = "Sushi Boom",
                Description = "Роллы и суши из свежих ингредиентов. Широкий выбор сетов и доставка по городу.",
                Address = "ул. Достык, 5, ТЦ Хан Шатыр", Phone = "+7 (717) 233-44-55",
                ImageUrl = "https://images.unsplash.com/photo-1553621042-f6e147245754?w=600&fit=crop",
                PriceRange = PriceRangeEnum.Medium, IsVerified = true,
                DistrictId = 3, CuisineId = 4, DateCreated = seedDate
            },
            new Restaurant
            {
                Id = 3, Title = "La Piazza",
                Description = "Итальянская пиццерия с дровяной печью. Паста, ризотто и живая музыка по пятницам.",
                Address = "пр. Туран, 24", Phone = "+7 (717) 255-66-77",
                ImageUrl = "https://images.unsplash.com/photo-1565299624946-b28f40a0ae38?w=600&fit=crop",
                PriceRange = PriceRangeEnum.Medium, IsVerified = true,
                DistrictId = 3, CuisineId = 3, DateCreated = seedDate
            },
            new Restaurant
            {
                Id = 4, Title = "Coffeemania",
                Description = "Авторский specialty-кофе, завтраки и десерты. Уютный интерьер для работы и встреч.",
                Address = "ул. Сыганак, 14", Phone = "+7 (717) 277-88-99",
                ImageUrl = "https://images.unsplash.com/photo-1495474472287-4d71bcdd2085?w=600&fit=crop",
                PriceRange = PriceRangeEnum.Medium, IsVerified = true,
                DistrictId = 3, CuisineId = 8, DateCreated = seedDate
            },
            new Restaurant
            {
                Id = 5, Title = "Чинара",
                Description = "Кавказская кухня: шашлыки, хинкали, хачапури. Большие порции и семейная атмосфера.",
                Address = "ул. Бейбітшілік, 18", Phone = "+7 (717) 211-22-33",
                ImageUrl = "https://images.unsplash.com/photo-1529193591184-b1d58069ecdd?w=600&fit=crop",
                PriceRange = PriceRangeEnum.Medium, IsVerified = true,
                DistrictId = 8, CuisineId = 6, DateCreated = seedDate
            },
            new Restaurant
            {
                Id = 6, Title = "Dragon Palace",
                Description = "Аутентичная китайская кухня: dim sum, утка по-пекински и морепродукты.",
                Address = "пр. Республики, 33", Phone = "+7 (717) 244-55-66",
                ImageUrl = "https://images.unsplash.com/photo-1563245372-f21724e3856d?w=600&fit=crop",
                PriceRange = PriceRangeEnum.Medium, IsVerified = false,
                DistrictId = 4, CuisineId = 5, DateCreated = seedDate
            },
            new Restaurant
            {
                Id = 7, Title = "Brasserie Française",
                Description = "Французская бистро-кухня: круассаны, стейк-фрит и бургундское вино. Живая музыка.",
                Address = "пр. Мәңгілік Ел, 55, Expo City", Phone = "+7 (717) 299-00-11",
                ImageUrl = "https://images.unsplash.com/photo-1414235077428-338989a2e8c0?w=600&fit=crop",
                PriceRange = PriceRangeEnum.Luxury, IsVerified = true,
                DistrictId = 3, CuisineId = 2, DateCreated = seedDate
            },
            new Restaurant
            {
                Id = 8, Title = "Burger House",
                Description = "Сочные бургеры из свежей говядины, картошка фри и молочные коктейли.",
                Address = "ул. Сарайшык, 7", Phone = "+7 (717) 222-33-44",
                ImageUrl = "https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=600&fit=crop",
                PriceRange = PriceRangeEnum.Budget, IsVerified = false,
                DistrictId = 5, CuisineId = 7, DateCreated = seedDate
            },
            new Restaurant
            {
                Id = 9, Title = "Алтын Дастархан",
                Description = "Традиционная казахская кухня: бешбармак, куырдак, манты. Национальный колорит и юрта.",
                Address = "ул. Иманбаева, 10", Phone = "+7 (717) 231-00-11",
                ImageUrl = "https://images.unsplash.com/photo-1504674900247-0877df9cc836?w=600&fit=crop",
                PriceRange = PriceRangeEnum.Medium, IsVerified = true,
                DistrictId = 1, CuisineId = 1, DateCreated = seedDate
            },
            new Restaurant
            {
                Id = 10, Title = "Sakura Japanese Restaurant",
                Description = "Премиальная японская кухня: темпура, омакасе-сеты, рамен и японские виски.",
                Address = "пр. Достык, 12", Phone = "+7 (717) 260-22-33",
                ImageUrl = "https://images.unsplash.com/photo-1617196034183-421b4040ed20?w=600&fit=crop",
                PriceRange = PriceRangeEnum.Premium, IsVerified = true,
                DistrictId = 4, CuisineId = 4, DateCreated = seedDate
            },
            new Restaurant
            {
                Id = 11, Title = "Mamma Mia",
                Description = "Семейная итальянская траттория. Паста ручной работы, тирамису и широкий выбор вин.",
                Address = "ул. Жубанова, 3", Phone = "+7 (717) 245-33-44",
                ImageUrl = "https://images.unsplash.com/photo-1473093295043-cdd812d0e601?w=600&fit=crop",
                PriceRange = PriceRangeEnum.Medium, IsVerified = false,
                DistrictId = 1, CuisineId = 3, DateCreated = seedDate
            },
            new Restaurant
            {
                Id = 12, Title = "Coffee Lab",
                Description = "Лаборатория кофе: эспрессо, фильтр, колд-брю. Бариста-чемпионы и авторские напитки.",
                Address = "ул. Кенесары, 40", Phone = "+7 (717) 212-55-66",
                ImageUrl = "https://images.unsplash.com/photo-1509042239860-f550ce710b93?w=600&fit=crop",
                PriceRange = PriceRangeEnum.Budget, IsVerified = true,
                DistrictId = 8, CuisineId = 8, DateCreated = seedDate
            },
            new Restaurant
            {
                Id = 13, Title = "Арагви",
                Description = "Грузинская и кавказская кухня. Хачапури по-аджарски, чахохбили и домашнее вино.",
                Address = "ул. Сейфуллина, 22", Phone = "+7 (717) 234-66-77",
                ImageUrl = "https://images.unsplash.com/photo-1528736235302-52922df5c122?w=600&fit=crop",
                PriceRange = PriceRangeEnum.Medium, IsVerified = false,
                DistrictId = 5, CuisineId = 6, DateCreated = seedDate
            },
            new Restaurant
            {
                Id = 14, Title = "Пекинская утка",
                Description = "Специализируемся на утке по-пекински. Лапша вок, пельмени цзяоцзы и чай пуэр.",
                Address = "пр. Абая, 15", Phone = "+7 (717) 278-77-88",
                ImageUrl = "https://images.unsplash.com/photo-1558030006-450675393462?w=600&fit=crop",
                PriceRange = PriceRangeEnum.Medium, IsVerified = false,
                DistrictId = 1, CuisineId = 5, DateCreated = seedDate
            },
            new Restaurant
            {
                Id = 15, Title = "Le Bistro",
                Description = "Европейская кухня в классическом стиле: телятина, лосось, крем-брюле. Бизнес-ланч.",
                Address = "пр. Кабанбай батыра, 58", Phone = "+7 (717) 291-88-99",
                ImageUrl = "https://images.unsplash.com/photo-1424847651672-bf20a4b0982b?w=600&fit=crop",
                PriceRange = PriceRangeEnum.Premium, IsVerified = true,
                DistrictId = 4, CuisineId = 2, DateCreated = seedDate
            },
            new Restaurant
            {
                Id = 16, Title = "KFC Астана",
                Description = "Сеть быстрого питания. Хрустящая курица, бургеры, картошка и напитки.",
                Address = "пр. Республики, 1, ТРЦ Мега", Phone = "+7 (800) 080-08-08",
                ImageUrl = "https://images.unsplash.com/photo-1562967914-608f82629710?w=600&fit=crop",
                PriceRange = PriceRangeEnum.Budget, IsVerified = true,
                DistrictId = 5, CuisineId = 7, DateCreated = seedDate
            },
            new Restaurant
            {
                Id = 17, Title = "Астана Гриль",
                Description = "Казахский гриль и мангал: рёбрышки, бараньи отбивные, шашлык. Открытая терраса.",
                Address = "ул. Аманжолова, 9", Phone = "+7 (717) 225-99-00",
                ImageUrl = "https://images.unsplash.com/photo-1555939594-58d7cb561ad1?w=600&fit=crop",
                PriceRange = PriceRangeEnum.Medium, IsVerified = false,
                DistrictId = 2, CuisineId = 1, DateCreated = seedDate
            },
            new Restaurant
            {
                Id = 18, Title = "Ramen House",
                Description = "Аутентичный японский рамен: тонкоцу, сёё, мисо. Гёза, тако-яки и японское пиво.",
                Address = "ул. Иманова, 18", Phone = "+7 (717) 256-11-22",
                ImageUrl = "https://images.unsplash.com/photo-1569050467447-ce54b3bbc37d?w=600&fit=crop",
                PriceRange = PriceRangeEnum.Medium, IsVerified = false,
                DistrictId = 6, CuisineId = 4, DateCreated = seedDate
            },
            new Restaurant
            {
                Id = 19, Title = "Pinocchio",
                Description = "Уютная пиццерия в итальянском стиле. Тонкое тесто, моцарелла, свежие овощи. Детское меню.",
                Address = "ул. Бейбітшілік, 55", Phone = "+7 (717) 267-22-33",
                ImageUrl = "https://images.unsplash.com/photo-1571407970349-bc81e7e96d47?w=600&fit=crop",
                PriceRange = PriceRangeEnum.Medium, IsVerified = true,
                DistrictId = 8, CuisineId = 3, DateCreated = seedDate
            },
            new Restaurant
            {
                Id = 20, Title = "Starbucks Expo",
                Description = "Кофейня Starbucks у комплекса Expo. Фраппучино, авторские латте и выпечка. Быстрый Wi-Fi.",
                Address = "пр. Мәңгілік Ел, 60, Expo", Phone = "+7 (717) 300-00-01",
                ImageUrl = "https://images.unsplash.com/photo-1461023058943-07fcbe16d735?w=600&fit=crop",
                PriceRange = PriceRangeEnum.Medium, IsVerified = true,
                DistrictId = 7, CuisineId = 8, DateCreated = seedDate
            }
        );
    }
}
