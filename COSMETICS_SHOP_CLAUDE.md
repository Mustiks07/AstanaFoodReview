# Cosmetics Shop — инструкция для Claude Code

## Что строим
Интернет-магазин косметики + платформа отзывов. Пользователи могут просматривать товары, добавлять в корзину, оформлять заказы и оставлять отзывы на продукты. Есть админ-панель для управления всем сайтом.

---

## Стек технологий
- **ASP.NET Core 10 MVC**
- **Entity Framework Core 10** + SQL Server
- **ASP.NET Identity** (роли: Admin, User)
- **Bootstrap 5.3** (CDN)
- **Кастомный CSS** (светлая тема, оранжевый бренд-цвет #F97316)
- **Переключатель языков RU/KZ** через cookie + LangService (словарь)

---

## Структура проекта

```
CosmeticsShop/
├── Controllers/
│   ├── HomeController.cs
│   ├── ProductsController.cs
│   ├── CartController.cs
│   ├── OrdersController.cs
│   ├── ReviewsController.cs
│   ├── AccountController.cs
│   ├── ProfileController.cs
│   ├── LanguageController.cs
│   └── Admin/
│       ├── Core.cs
│       ├── Products.cs
│       ├── Orders.cs
│       ├── Reviews.cs
│       ├── Brands.cs
│       └── Categories.cs
├── Domain/
│   ├── AppDbContext.cs
│   ├── Entities/
│   │   ├── Product.cs
│   │   ├── Brand.cs
│   │   ├── Category.cs
│   │   ├── Review.cs
│   │   ├── ReviewVote.cs
│   │   ├── Cart.cs
│   │   ├── CartItem.cs
│   │   ├── Order.cs
│   │   └── OrderItem.cs
│   ├── Enums/
│   │   ├── PriceRangeEnum.cs
│   │   ├── OrderStatusEnum.cs
│   │   └── ReviewVoteTypeEnum.cs
│   └── Repositories/
│       ├── Abstract/ (интерфейсы)
│       └── EntityFramework/ (реализации)
├── Infrastructure/
│   ├── DataManager.cs
│   └── HelperDTO.cs
├── Models/ (ViewModels + DTOs)
├── Services/
│   ├── LangService.cs
│   └── DbInitializer.cs
├── Views/
│   ├── Home/
│   ├── Products/
│   ├── Cart/
│   ├── Orders/
│   ├── Reviews/
│   ├── Account/
│   ├── Profile/
│   ├── Admin/
│   └── Shared/
└── wwwroot/
    ├── css/site.css
    └── js/
```

---

## Entities (сущности базы данных)

### Brand (Бренд)
```csharp
public class Brand
{
    public int Id { get; set; }
    public string Title { get; set; }        // L'Oréal, MAC, Dior
    public string? Description { get; set; }
    public string? Country { get; set; }     // Франция, США
    public string? LogoUrl { get; set; }
    public bool IsVerified { get; set; }
    public ICollection<Product> Products { get; set; }
}
```

### Category (Категория)
```csharp
public class Category
{
    public int Id { get; set; }
    public string Title { get; set; }        // Уход за лицом, Макияж
    public string? IconEmoji { get; set; }   // 💄 🧴 💅
    public ICollection<Product> Products { get; set; }
}
```

### Product (Товар)
```csharp
public class Product
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public string? Volume { get; set; }      // 50ml, 30g
    public string? SkinType { get; set; }   // Все типы, Сухая, Жирная
    public bool InStock { get; set; }
    public bool IsVerified { get; set; }
    public bool IsBestseller { get; set; }
    public PriceRangeEnum PriceRange { get; set; }
    public DateTime DateCreated { get; set; }
    public int BrandId { get; set; }
    public Brand Brand { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public ICollection<Review> Reviews { get; set; }
    // Вычисляемые:
    // AverageRating — среднее из Reviews
    // ReviewCount   — кол-во отзывов
}
```

### Review (Отзыв)
```csharp
public class Review
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Text { get; set; }
    public int Rating { get; set; }          // 1-5 общий
    public int QualityRating { get; set; }   // 1-5 качество
    public int PackagingRating { get; set; } // 1-5 упаковка
    public int ValueRating { get; set; }     // 1-5 цена/качество
    public bool VerifiedPurchase { get; set; }
    public DateTime DateCreated { get; set; }
    public string AuthorId { get; set; }
    public IdentityUser Author { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public ICollection<ReviewVote> Votes { get; set; }
}
```

### ReviewVote (Голос за отзыв)
```csharp
public class ReviewVote
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int ReviewId { get; set; }
    public ReviewVoteTypeEnum VoteType { get; set; } // Useful, NotUseful
}
```

### Cart (Корзина)
```csharp
public class Cart
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public IdentityUser User { get; set; }
    public ICollection<CartItem> Items { get; set; }
    // Вычисляемое: Total = Items.Sum(i => i.Product.Price * i.Quantity)
}
```

### CartItem (Позиция в корзине)
```csharp
public class CartItem
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public Cart Cart { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
}
```

### Order (Заказ)
```csharp
public class Order
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public IdentityUser User { get; set; }
    public OrderStatusEnum Status { get; set; }
    public DateTime DateCreated { get; set; }
    public string DeliveryAddress { get; set; }
    public string Phone { get; set; }
    public string? Comment { get; set; }
    public decimal Total { get; set; }
    public ICollection<OrderItem> Items { get; set; }
}
```

### OrderItem (Позиция заказа)
```csharp
public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public decimal PriceAtPurchase { get; set; } // цена на момент заказа
}
```

---

## Enums

```csharp
public enum PriceRangeEnum
{
    Budget   = 0,  // до 3000 ₸
    Medium   = 1,  // 3000–10000 ₸
    Premium  = 2,  // 10000–30000 ₸
    Luxury   = 3   // от 30000 ₸
}

public enum OrderStatusEnum
{
    Pending    = 0,  // Ожидает подтверждения
    Processing = 1,  // Обрабатывается
    Shipped    = 2,  // Отправлен
    Delivered  = 3,  // Доставлен
    Cancelled  = 4   // Отменён
}

public enum ReviewVoteTypeEnum
{
    Useful    = 0,
    NotUseful = 1
}
```

---

## Seed данные (начальные данные в AppDbContext.OnModelCreating)

### Категории (8 штук)
```
1. Уход за лицом        💧
2. Декоративная косметика 💄
3. Уход за волосами      💇
4. Парфюмерия            🌸
5. Уход за телом         🧴
6. Средства для ногтей   💅
7. Солнцезащитные        ☀️
8. Инструменты и кисти   🖌️
```

### Бренды (10 штук)
```
1.  L'Oréal Paris    — Франция
2.  Maybelline       — США
3.  MAC Cosmetics    — Канада
4.  The Ordinary     — Канада
5.  CeraVe           — США
6.  Dior Beauty      — Франция
7.  Chanel Beauty    — Франция
8.  Nivea            — Германия
9.  Garnier          — Франция
10. NYX Professional — США
```

### Товары (20 штук с реальными Unsplash фото)
Примеры ImageUrl для косметики:
```
https://images.unsplash.com/photo-1596462502278-27bfdc403348?w=600&fit=crop  (помада)
https://images.unsplash.com/photo-1631730486572-226d1f595b68?w=600&fit=crop  (крем)
https://images.unsplash.com/photo-1612817288484-6f916006741a?w=600&fit=crop  (тушь)
https://images.unsplash.com/photo-1571781926291-c477ebfd024b?w=600&fit=crop  (палетка)
https://images.unsplash.com/photo-1620916566398-39f1143ab7be?w=600&fit=crop  (сыворотка)
https://images.unsplash.com/photo-1583241475880-083f84372725?w=600&fit=crop  (духи)
https://images.unsplash.com/photo-1599305445671-ac291c95aaa9?w=600&fit=crop  (косметика)
https://images.unsplash.com/photo-1522338242992-e1a54906a8da?w=600&fit=crop  (кисти)
```

Набор товаров для seed (по 2-3 от каждой категории):
- Помада матовая Rouge L'Oréal — 4500₸ — Макияж
- Тушь для ресниц Maybelline Sky High — 3200₸ — Макияж
- Тональный крем MAC Studio Fix — 18000₸ — Макияж
- Сыворотка с витамином C The Ordinary — 6500₸ — Уход за лицом
- Увлажняющий крем CeraVe — 5800₸ — Уход за лицом
- Мицеллярная вода Garnier — 2100₸ — Уход за лицом
- Солнцезащитный крем SPF50 Nivea — 3900₸ — Солнцезащитные
- Шампунь L'Oréal Elseve — 2800₸ — Уход за волосами
- Маска для волос Garnier — 1900₸ — Уход за волосами
- Парфюм Dior Miss Dior — 65000₸ — Парфюмерия
- Парфюм Chanel No.5 — 89000₸ — Парфюмерия
- Лак для ногтей NYX — 1800₸ — Ногти
- Гель для душа Nivea — 1500₸ — Уход за телом
- Скраб для тела L'Oréal — 4200₸ — Уход за телом
- Набор кистей MAC — 22000₸ — Инструменты
- Консилер NYX — 3100₸ — Макияж
- Хайлайтер Maybelline — 2900₸ — Макияж
- Тоник для лица The Ordinary — 4800₸ — Уход за лицом
- Крем для рук Nivea — 900₸ — Уход за телом
- Блеск для губ NYX — 2400₸ — Макияж

### Admin пользователь
```csharp
// Username: admin
// Password: Admin@Beauty2026!
// Role: Admin
// Seed через HasData + DbInitializer (как в AstanaFoodReview)
```

---

## Страницы и маршруты

### Публичные страницы
```
GET  /                          — Главная (hero, stats, bestsellers, categories)
GET  /Products                  — Каталог товаров (фильтры)
GET  /Products/Show/{id}        — Карточка товара + отзывы
GET  /Top                       — Топ товаров по рейтингу
GET  /Brands                    — Все бренды
GET  /Brands/{id}               — Товары бренда
GET  /account/login             — Вход (обычные пользователи)
GET  /account/register          — Регистрация
```

### Для авторизованных пользователей
```
GET  /cart                      — Корзина
POST /cart/add                  — Добавить в корзину
POST /cart/remove               — Удалить из корзины
POST /cart/update               — Изменить количество
GET  /orders                    — Мои заказы
GET  /orders/{id}               — Детали заказа
POST /orders/checkout           — Оформить заказ
GET  /profile                   — Профиль + история заказов
POST /reviews/new               — Оставить отзыв
POST /reviews/vote              — Голосовать за отзыв
POST /reviews/delete/{id}       — Удалить отзыв
```

### Адмін панель (отдельный логин /admin/login)
```
GET  /admin                     — Dashboard (статистика)
GET  /admin/products            — Список товаров
GET  /admin/products/new        — Добавить товар
GET  /admin/products/edit/{id}  — Редактировать товар
POST /admin/products/delete/{id}— Удалить товар
GET  /admin/orders              — Все заказы
POST /admin/orders/status/{id}  — Изменить статус заказа
GET  /admin/reviews             — Все отзывы
POST /admin/reviews/delete/{id} — Удалить отзыв
GET  /admin/brands              — Бренды
GET  /admin/brands/new          — Добавить бренд
GET  /admin/categories          — Категории
GET  /admin/login               — Вход в админку (отдельная страница)
```

---

## Функциональность по шагам

### 1. Каталог товаров (`/Products`)
Фильтры:
- Поиск по названию
- Категория (select)
- Бренд (select)
- Ценовой диапазон (select: до 3000, 3000-10000, 10000-30000, от 30000)
- Тип кожи (select: все, сухая, жирная, комбинированная, чувствительная)
- Сортировка: новые / рейтинг / цена ↑ / цена ↓
- Только в наличии (checkbox)

Карточка товара:
- Фото товара
- Название + бренд
- Категория
- Цена
- Звёзды рейтинга + кол-во отзывов
- Бейдж "Bestseller" если IsBestseller=true
- Бейдж "Нет в наличии" если InStock=false
- Кнопка "В корзину" (если авторизован)

### 2. Карточка товара (`/Products/Show/{id}`)
Левая колонка:
- Большое фото товара
- Название, бренд, категория
- Цена
- Объём/вес
- Тип кожи
- Состав/описание
- Кнопка "В корзину" + счётчик количества
- Кнопка "Купить сейчас" (сразу checkout)

Правая колонка (сайдбар):
- Общий рейтинг (прогресс-бар)
- Разбивка: Качество / Упаковка / Цена-качество
- Ссылка "Написать отзыв"

Внизу — отзывы:
- Автор, дата, рейтинг
- Текст отзыва
- Подрейтинги: Качество / Упаковка / Цена-качество
- Кнопки 👍 Полезно / 👎 Не полезно
- Бейдж "Подтверждённая покупка" если VerifiedPurchase=true
- Кнопка удалить (для автора или админа)

### 3. Корзина (`/cart`)
- Список товаров с фото, названием, ценой
- Изменение количества (+ / -)
- Удаление позиции
- Итого сумма
- Кнопка "Оформить заказ"

### 4. Оформление заказа (`/orders/checkout`)
Форма:
- Адрес доставки
- Телефон
- Комментарий к заказу
- Итого с перечнем товаров
- Кнопка "Подтвердить заказ"

После подтверждения:
- Создаётся Order со статусом Pending
- Cart очищается
- Если пользователь купил товар — ReviewVote.VerifiedPurchase=true
- Редирект на страницу заказа с номером

### 5. Профиль (`/profile`)
- Данные пользователя
- История заказов (список с датой, суммой, статусом)
- Мои отзывы

### 6. Форма отзыва (`/reviews/new/{productId}`)
- Заголовок отзыва *
- Текст (необязательно)
- Общая оценка ★ (1-5)
- Качество ★ (1-5)
- Упаковка ★ (1-5)
- Цена/Качество ★ (1-5)
- Кнопка "Опубликовать"

Только авторизованные пользователи могут писать отзывы.
VerifiedPurchase=true автоматически если пользователь заказывал этот товар.

### 7. Админ панель
Dashboard показывает:
- Всего товаров
- Всего заказов
- Заказы за сегодня
- Выручка за месяц
- Последние 5 заказов
- Новые отзывы

Управление заказами:
- Таблица всех заказов (номер, пользователь, сумма, статус, дата)
- Изменение статуса: Pending → Processing → Shipped → Delivered / Cancelled

---

## ViewModels (модели для форм и отображения)

```csharp
// Для каталога
public class ProductDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string BrandName { get; set; }
    public string CategoryName { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public string? Volume { get; set; }
    public bool InStock { get; set; }
    public bool IsBestseller { get; set; }
    public bool IsVerified { get; set; }
    public double AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public string PriceRangeLabel { get; set; }
}

// Для добавления товара (Admin)
public class CreateProductViewModel
{
    [Required] public string Title { get; set; }
    public string? Description { get; set; }
    [Required] public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public string? Volume { get; set; }
    public string? SkinType { get; set; }
    public bool InStock { get; set; } = true;
    public bool IsBestseller { get; set; }
    [Required] public int BrandId { get; set; }
    [Required] public int CategoryId { get; set; }
    public PriceRangeEnum PriceRange { get; set; }
    public IEnumerable<Brand> Brands { get; set; }
    public IEnumerable<Category> Categories { get; set; }
}

// Для отзыва
public class CreateReviewViewModel
{
    [Required] public int ProductId { get; set; }
    [Required] public string Title { get; set; }
    public string? Text { get; set; }
    [Range(1, 5)] public int Rating { get; set; } = 5;
    [Range(1, 5)] public int QualityRating { get; set; } = 5;
    [Range(1, 5)] public int PackagingRating { get; set; } = 5;
    [Range(1, 5)] public int ValueRating { get; set; } = 5;
}

// Для оформления заказа
public class CheckoutViewModel
{
    [Required] public string DeliveryAddress { get; set; }
    [Required] public string Phone { get; set; }
    public string? Comment { get; set; }
    public List<CartItem> Items { get; set; }
    public decimal Total { get; set; }
}

// Для отображения заказа
public class OrderDTO
{
    public int Id { get; set; }
    public string StatusLabel { get; set; }
    public string StatusColor { get; set; }  // CSS цвет
    public DateTime DateCreated { get; set; }
    public decimal Total { get; set; }
    public string DeliveryAddress { get; set; }
    public List<OrderItemDTO> Items { get; set; }
}
```

---

## LangService — словарь RU/KZ

Все строки интерфейса через `LangService.T("key")`. Ключи:

```
nav.products, nav.brands, nav.cart, nav.orders, nav.profile
nav.login, nav.register, nav.logout, nav.admin

home.subtitle, home.find, home.bestsellers, home.by_category

prod.title, prod.add_to_cart, prod.buy_now, prod.out_of_stock
prod.bestseller, prod.verified, prod.reviews_sfx, prod.in_stock
prod.search, prod.all_brands, prod.all_cats, prod.any_price
prod.sort_new, prod.sort_rating, prod.sort_price_asc, prod.sort_price_desc

cart.title, cart.empty, cart.total, cart.checkout, cart.remove
cart.quantity

order.title, order.address, order.phone, order.comment
order.confirm, order.status_pending, order.status_processing
order.status_shipped, order.status_delivered, order.status_cancelled
order.my_orders, order.number, order.date, order.total

rev.page_title, rev.title_label, rev.text_label, rev.rating
rev.quality, rev.packaging, rev.value_price, rev.submit
rev.verified_purchase, rev.useful, rev.not_useful, rev.delete
rev.no_reviews, rev.be_first

price.budget, price.medium, price.premium, price.luxury
```

---

## CSS стиль (такой же как AstanaFoodReview)

Скопировать подход из AstanaFoodReview/wwwroot/css/site.css:

```css
:root {
    --brand:       #F97316;  /* оранжевый — главный цвет */
    --brand-dark:  #EA6C0A;
    --brand-muted: rgba(249,115,22,0.10);
    --bg:          #F8FAFC;
    --bg-white:    #FFFFFF;
    --border:      #E2E8F0;
    --text:        #1E293B;
    --text-muted:  #64748B;
    --radius:      12px;
    --shadow:      0 4px 16px rgba(0,0,0,0.08);
}
```

Компоненты: `.product-card`, `.brand-pill`, `.badge-price`, `.badge-bestseller`, `.cart-item`, `.order-card`, `.star-input`, `.lang-btn`, `.admin-table`

Карточка товара аналогична `.rest-card` из AstanaFoodReview.

---

## Безопасность

- `[Authorize]` на CartController, OrdersController, ReviewsController
- `[Authorize(Roles = "Admin")]` на всех Admin контроллерах
- Отдельная страница `/admin/login` — только для admin роли
- `/account/login` — только для обычных пользователей
- Если авторизован → редирект с login/register страниц на главную
- `ValidateAntiForgeryToken` на всех POST
- DbInitializer меняет пароль admin с дефолтного на `Admin@Beauty2026!`

---

## Program.cs — регистрация сервисов

```csharp
builder.Services.AddDbContext<AppDbContext>(...);
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath        = "/account/login";
    options.AccessDeniedPath = "/account/accessdenied";
    options.Events.OnRedirectToLogin = ctx =>
    {
        ctx.Response.Redirect(
            ctx.Request.Path.StartsWithSegments("/admin")
                ? "/admin/login"
                : $"/account/login?ReturnUrl={Uri.EscapeDataString(ctx.Request.Path)}");
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = ctx =>
    {
        ctx.Response.Redirect(
            ctx.Request.Path.StartsWithSegments("/admin")
                ? "/admin/login"
                : "/account/accessdenied");
        return Task.CompletedTask;
    };
});
builder.Services.AddScoped<IProductsRepository, EFProductsRepository>();
builder.Services.AddScoped<IBrandsRepository, EFBrandsRepository>();
builder.Services.AddScoped<ICategoriesRepository, EFCategoriesRepository>();
builder.Services.AddScoped<IReviewsRepository, EFReviewsRepository>();
builder.Services.AddScoped<ICartRepository, EFCartRepository>();
builder.Services.AddScoped<IOrdersRepository, EFOrdersRepository>();
builder.Services.AddScoped<DataManager>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<LangService>();
builder.Services.AddControllersWithViews();
```

---

## appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=localhost;Database=CosmeticsShop;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True"
  },
  "Project": {
    "Site": {
      "Name": "Beauty Shop",
      "City": "Астана"
    }
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

---

## Порядок разработки

1. Создать проект: `dotnet new mvc -n CosmeticsShop`
2. Установить пакеты:
   ```
   dotnet add package Microsoft.EntityFrameworkCore.SqlServer
   dotnet add package Microsoft.EntityFrameworkCore.Design
   dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
   ```
3. Создать все Entities
4. Создать AppDbContext с HasData (seed)
5. Создать Repositories (Abstract + EF)
6. Создать DataManager
7. Настроить Program.cs
8. Создать ViewModels и DTOs
9. Создать LangService (RU/KZ словарь)
10. Создать все Controllers
11. Создать все Views
12. Создать wwwroot/css/site.css
13. Создать DbInitializer
14. `dotnet ef migrations add Initial`
15. `dotnet ef database update`
16. `dotnet run` и проверить

---

## Главная страница (`/`) — что показывать

- **Hero секция**: заголовок + подзаголовок + кнопки "Каталог" и "Регистрация"
- **Статистика**: Товаров / Брендов / Средний рейтинг
- **Bestsellers**: топ 4 товара с IsBestseller=true
- **Категории**: горизонтальный ряд кнопок-пилюль с эмодзи
- **Бренды**: горизонтальный ряд логотипов/названий
- **CTA блок**: для неавторизованных — призыв к регистрации

---

## Важные замечания для Claude Code

1. Все ссылки на детальную страницу товара: `/Products/Show/{id}` (НЕ `/products/{id}`)
2. POST формы с атрибутными маршрутами использовать `action="/reviews/new"` а не tag helper
3. Admin контроллеры называть `AdminProductsController`, `AdminOrdersController` (НЕ просто `Products`)
4. `_ViewImports.cshtml` должен иметь `@inject LangService L`
5. Все admin views использовать явный путь `View("~/Views/Admin/...")`
6. Перед `dotnet run` обязательно `dotnet ef database update`
7. `appsettings.Development.json` — локальный файл, не пушить в git (добавить в .gitignore)
8. При ошибке "SSL certificate" добавить `TrustServerCertificate=True` в строку подключения
9. VerifiedPurchase у отзыва — автоматически true если пользователь сделал заказ с этим товаром
10. Количество в корзине хранить в CartItem.Quantity, не создавать дубли записей
