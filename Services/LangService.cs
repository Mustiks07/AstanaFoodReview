namespace AstanaFoodReviews.Services;

public class LangService
{
    private readonly string _lang;

    private static readonly Dictionary<string, Dictionary<string, string>> _dict = new()
    {
        ["ru"] = new()
        {
            // Navbar
            ["nav.restaurants"]   = "Заведения",
            ["nav.top"]           = "Топ",
            ["nav.map"]           = "Карта",
            ["nav.about"]         = "О нас",
            ["nav.profile"]       = "Профиль",
            ["nav.admin"]         = "Админ",
            ["nav.logout"]        = "Выйти",
            ["nav.login"]         = "Войти",
            ["nav.register"]      = "Регистрация",
            // Home
            ["home.subtitle"]     = "Честные отзывы о ресторанах и кафе Астаны от реальных жителей города",
            ["home.find"]         = "Найти заведение",
            ["home.write_review"] = "Написать отзыв",
            ["home.stat_rest"]    = "Заведений",
            ["home.stat_rev"]     = "Отзывов",
            ["home.stat_avg"]     = "Средний рейтинг",
            ["home.best"]         = "Лучшие заведения",
            ["home.all_rating"]   = "Весь рейтинг →",
            ["home.by_cuisine"]   = "Выбрать по кухне",
            ["home.been_there"]   = "Были в ресторане?",
            ["home.cta_body"]     = "Зарегистрируйтесь и помогите другим жителям Астаны выбрать лучшее заведение",
            // Restaurants list
            ["rest.title"]        = "Заведения Астаны",
            ["rest.add"]          = "+ Добавить",
            ["rest.search"]       = "Поиск по названию...",
            ["rest.all_dist"]     = "Все районы",
            ["rest.all_cui"]      = "Все кухни",
            ["rest.any_price"]    = "Любая цена",
            ["rest.sort_new"]     = "Новые",
            ["rest.sort_rating"]  = "Рейтинг",
            ["rest.sort_reviews"] = "Отзывы",
            ["rest.find_btn"]     = "Найти",
            ["rest.not_found"]    = "Заведения не найдены",
            ["rest.verified"]     = "✓ Верифицировано",
            ["rest.reviews_sfx"]  = "отз.",
            // Price labels
            ["price.budget"]      = "до 2000 ₸",
            ["price.medium"]      = "2000–5000 ₸",
            ["price.premium"]     = "5000–15000 ₸",
            ["price.luxury"]      = "от 15000 ₸",
            // Map
            ["map.title"]         = "Карта заведений",
            ["map.sfx"]           = "заведений",
            // Top
            ["top.title"]         = "Топ заведений Астаны",
            ["top.empty"]         = "Нет данных",
            // Show (restaurant detail)
            ["show.reviews"]      = "Отзывы",
            ["show.leave"]        = "+ Оставить отзыв",
            ["show.login_write"]  = "Войти, чтобы написать",
            ["show.write"]        = "+ Написать отзыв",
            ["show.login_cta"]    = "Войдите, чтобы оставить отзыв",
            ["show.login"]        = "Войти",
            ["show.rating"]       = "Рейтинг",
            ["show.total"]        = "Общий",
            ["show.of5"]          = "из 5",
            ["show.edit"]         = "⚙ Редактировать",
            ["show.no_reviews"]   = "Отзывов пока нет. Будьте первым!",
            ["show.food"]         = "Еда",
            ["show.service"]      = "Сервис",
            ["show.price_q"]      = "Цена/кач-во",
            ["show.useful"]       = "Полезно",
            ["show.not_useful"]   = "Не полезно",
            ["show.delete"]       = "Удалить",
            ["show.owner_resp"]   = "Ответ заведения от",
            ["show.rev_count"]    = "отзывов",
        },
        ["kz"] = new()
        {
            // Navbar
            ["nav.restaurants"]   = "Мекемелер",
            ["nav.top"]           = "Топ",
            ["nav.map"]           = "Карта",
            ["nav.about"]         = "Біз туралы",
            ["nav.profile"]       = "Профиль",
            ["nav.admin"]         = "Әкімші",
            ["nav.logout"]        = "Шығу",
            ["nav.login"]         = "Кіру",
            ["nav.register"]      = "Тіркелу",
            // Home
            ["home.subtitle"]     = "Астана тұрғындарының мейрамханалар мен кафелер туралы шынайы пікірлері",
            ["home.find"]         = "Мекеме табу",
            ["home.write_review"] = "Пікір жазу",
            ["home.stat_rest"]    = "Мекеме",
            ["home.stat_rev"]     = "Пікір",
            ["home.stat_avg"]     = "Орташа рейтинг",
            ["home.best"]         = "Үздік мекемелер",
            ["home.all_rating"]   = "Барлық рейтинг →",
            ["home.by_cuisine"]   = "Асхана бойынша таңдау",
            ["home.been_there"]   = "Мейрамханада болдыңыз ба?",
            ["home.cta_body"]     = "Тіркеліп, Астана тұрғындарына үздік мекемені таңдауға көмектесіңіз",
            // Restaurants list
            ["rest.title"]        = "Астана мекемелері",
            ["rest.add"]          = "+ Қосу",
            ["rest.search"]       = "Атауы бойынша іздеу...",
            ["rest.all_dist"]     = "Барлық аудандар",
            ["rest.all_cui"]      = "Барлық асханалар",
            ["rest.any_price"]    = "Кез келген баға",
            ["rest.sort_new"]     = "Жаңалар",
            ["rest.sort_rating"]  = "Рейтинг",
            ["rest.sort_reviews"] = "Пікірлер",
            ["rest.find_btn"]     = "Іздеу",
            ["rest.not_found"]    = "Мекеме табылмады",
            ["rest.verified"]     = "✓ Расталған",
            ["rest.reviews_sfx"]  = "пікір",
            // Price labels
            ["price.budget"]      = "2000 ₸ дейін",
            ["price.medium"]      = "2000–5000 ₸",
            ["price.premium"]     = "5000–15000 ₸",
            ["price.luxury"]      = "15000 ₸ жоғары",
            // Map
            ["map.title"]         = "Мекемелер картасы",
            ["map.sfx"]           = "мекеме",
            // Top
            ["top.title"]         = "Астана мекемелерінің топы",
            ["top.empty"]         = "Деректер жоқ",
            // Show
            ["show.reviews"]      = "Пікірлер",
            ["show.leave"]        = "+ Пікір қалдыру",
            ["show.login_write"]  = "Жазу үшін кіру",
            ["show.write"]        = "+ Пікір жазу",
            ["show.login_cta"]    = "Пікір қалдыру үшін кіріңіз",
            ["show.login"]        = "Кіру",
            ["show.rating"]       = "Рейтинг",
            ["show.total"]        = "Жалпы",
            ["show.of5"]          = "5-тен",
            ["show.edit"]         = "⚙ Өңдеу",
            ["show.no_reviews"]   = "Әзірге пікір жоқ. Бірінші болыңыз!",
            ["show.food"]         = "Тағам",
            ["show.service"]      = "Қызмет",
            ["show.price_q"]      = "Баға/сапа",
            ["show.useful"]       = "Пайдалы",
            ["show.not_useful"]   = "Пайдасыз",
            ["show.delete"]       = "Жою",
            ["show.owner_resp"]   = "Мекеме жауабы",
            ["show.rev_count"]    = "пікір",
        }
    };

    public LangService(IHttpContextAccessor accessor)
    {
        var cookie = accessor.HttpContext?.Request.Cookies["lang"];
        _lang = cookie == "kz" ? "kz" : "ru";
    }

    public string Lang => _lang;

    public string T(string key)
    {
        if (_dict.TryGetValue(_lang, out var d) && d.TryGetValue(key, out var v))
            return v;
        if (_dict["ru"].TryGetValue(key, out var ru))
            return ru;
        return key;
    }
}
