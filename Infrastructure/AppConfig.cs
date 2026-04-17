namespace AstanaFoodReviews.Infrastructure;

public class AppConfig
{
    public SiteConfig Site { get; set; } = new();
}

public class SiteConfig
{
    public string Name { get; set; } = "Astana Food Reviews";
    public string City { get; set; } = "Астана";
}
