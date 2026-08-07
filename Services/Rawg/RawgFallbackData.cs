namespace QuestLog.Services.Rawg;

// Local offline dataset used when RAWG.io is unreachable, so Explore keeps working
// (browse, filter, add to shelf) with demo data instead of a hard failure.
public static class RawgFallbackData
{
    private const int Action = 4;
    private const int Adventure = 3;
    private const int Rpg = 5;
    private const int Strategy = 10;
    private const int Shooter = 2;
    private const int Indie = 51;
    private const int Casual = 40;
    private const int Simulation = 14;
    private const int Sports = 15;
    private const int Racing = 1;

    public static readonly List<RawgGenreDto> Genres =
        new()
        {
            new() { Id = Action, Name = "Action" },
            new() { Id = Adventure, Name = "Adventure" },
            new() { Id = Rpg, Name = "RPG" },
            new() { Id = Strategy, Name = "Strategy" },
            new() { Id = Shooter, Name = "Shooter" },
            new() { Id = Indie, Name = "Indie" },
            new() { Id = Casual, Name = "Casual" },
            new() { Id = Simulation, Name = "Simulation" },
            new() { Id = Sports, Name = "Sports" },
            new() { Id = Racing, Name = "Racing" },
        };

    private const int Pc = 4;
    private const int Ps5 = 187;
    private const int Ps4 = 18;
    private const int XboxSeries = 186;
    private const int XboxOne = 1;
    private const int Switch = 7;
    private const int MacOs = 5;
    private const int Linux = 6;
    private const int Ios = 3;
    private const int Android = 21;

    public static readonly List<RawgPlatformDto> Platforms =
        new()
        {
            new() { Id = Pc, Name = "PC" },
            new() { Id = Ps5, Name = "PlayStation 5" },
            new() { Id = Ps4, Name = "PlayStation 4" },
            new() { Id = XboxSeries, Name = "Xbox Series S/X" },
            new() { Id = XboxOne, Name = "Xbox One" },
            new() { Id = Switch, Name = "Nintendo Switch" },
            new() { Id = MacOs, Name = "macOS" },
            new() { Id = Linux, Name = "Linux" },
            new() { Id = Ios, Name = "iOS" },
            new() { Id = Android, Name = "Android" },
        };

    private static readonly Dictionary<int, string> PlatformNamesById = Platforms.ToDictionary(
        p => p.Id,
        p => p.Name
    );

    private static string Cover(string label, string color) =>
        "data:image/svg+xml,"
        + Uri.EscapeDataString(
            $"<svg xmlns='http://www.w3.org/2000/svg' width='400' height='225'>"
                + $"<rect width='100%' height='100%' fill='{color}'/>"
                + $"<text x='50%' y='50%' font-family='sans-serif' font-size='28' fill='white' "
                + $"text-anchor='middle' dominant-baseline='middle'>{label}</text></svg>"
        );

    private static List<PlatformContainerDto> On(params int[] platformIds) =>
        platformIds
            .Select(
                id =>
                    new PlatformContainerDto
                    {
                        Platform = new PlatformDto { Name = PlatformNamesById[id] }
                    }
            )
            .ToList();

    public static readonly List<RawgGameDto> Games =
        new()
        {
            new()
            {
                Id = 100001,
                Name = "The Witcher 3: Wild Hunt",
                Background_Image =
                    "https://store-images.s-microsoft.com/image/apps.46303.65858607118306853.39ed2a08-df0d-4ae1-aee0-c66ffb783a34.1fbbd7b6-6399-4b79-99f0-f48c6ada8a2b?q=90&w=480&h=270",
                Metacritic = 92,
                Platforms = On(Pc, Ps4, XboxOne, Switch)
            },
            new()
            {
                Id = 100002,
                Name = "Hades",
                Background_Image =
                    "https://i0.wp.com/screenrex.com/wp-content/uploads/2020/12/Hades_Aug19_04.png?resize=1440%2C1080&ssl=1",
                Metacritic = 93,
                Platforms = On(Pc, Ps5, Switch, MacOs)
            },
            new()
            {
                Id = 100003,
                Name = "Stardew Valley",
                Background_Image = "https://i.blogs.es/c26d6a/20240325180553_1/1200_900.jpeg",
                Metacritic = 89,
                Platforms = On(Pc, Switch, Ios, Android)
            },
            new()
            {
                Id = 100004,
                Name = "Portal 2",
                Background_Image =
                    "https://gfn.ru/media/images/art_image-portal-2-7150b35a.original.jpg",
                Metacritic = 95,
                Platforms = On(Pc, MacOs, Linux)
            },
            new()
            {
                Id = 100005,
                Name = "Elden Ring",
                Background_Image =
                    "https://nintenduo.com/wp-content/uploads/2025/08/Elden-Ring-Cover.webp",
                Metacritic = 96,
                Platforms = On(Pc, Ps5, XboxSeries)
            },
            new()
            {
                Id = 100006,
                Name = "Celeste",
                Background_Image =
                    "https://ewingsvoice.com/wp-content/uploads/2019/11/480274-celeste-nintendo-switch-front-cover.jpg",
                Metacritic = 92,
                Platforms = On(Pc, Switch, Ps4, XboxOne)
            },
            new()
            {
                Id = 100007,
                Name = "Hollow Knight",
                Background_Image =
                    "https://images.squarespace-cdn.com/content/v1/606d159a953867291018f801/6cf9fc95-a97a-42dd-840b-e4a48e26637b/HK_header.jpg",
                Metacritic = 90,
                Platforms = On(Pc, Switch, Ps4, XboxOne)
            },
            new()
            {
                Id = 100008,
                Name = "Baldur's Gate 3",
                Background_Image =
                    "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1086940/59827b3d0abf2f29adacfe72fdfd11059d6974e2/capsule_616x353.jpg?t=1777363040",
                Metacritic = 96,
                Platforms = On(Pc, Ps5, XboxSeries)
            },
            new()
            {
                Id = 100009,
                Name = "Minecraft",
                Background_Image =
                    "https://store-images.s-microsoft.com/image/apps.608.13510798885735219.cf55aeca-e690-41e0-a88b-41b0e517a3be.c94e1bfa-1b68-4cf5-9954-f967168480b4?q=90&w=480&h=270",
                Metacritic = 93,
                Platforms = On(Pc, Ps4, XboxOne, Switch, Ios, Android)
            },
            new()
            {
                Id = 100010,
                Name = "Half-Life 2",
                Background_Image =
                    "https://mediaproxy.tvtropes.org/width/1200/https://static.tvtropes.org/pmwiki/pub/images/hl2_gordon_alyx_citadel.jpg",
                Metacritic = 96,
                Platforms = On(Pc, MacOs, Linux)
            },
            new()
            {
                Id = 100011,
                Name = "Slay the Spire",
                Background_Image =
                    "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQjY5QdyXWdUo0jqYuCLE_nidOUgafXTfuSQj6mOmu-muf1Y3vPeVU5D7-V&s=10",
                Metacritic = 87,
                Platforms = On(Pc, Switch, Ps4, Ios)
            },
            new()
            {
                Id = 100012,
                Name = "Civilization VI",
                Background_Image =
                    "https://cdn.akamai.steamstatic.com/steamcommunity/public/images/clans/25030453/9ddded1b1ed3eb5e4581b7ca70f1482821dfbcbc.jpg",
                Metacritic = 88,
                Platforms = On(Pc, Switch, XboxOne, Ios)
            },
            new()
            {
                Id = 100013,
                Name = "Fortnite",
                Background_Image =
                    "https://static.wikia.nocookie.net/fortnite/images/a/ae/Fortnite_%28Update_v28.00%29_-_Cover_Art_-_Fortnite.jpg/revision/latest?cb=20231203094112",
                Metacritic = 78,
                Platforms = On(Pc, Ps5, XboxSeries, Switch, Ios, Android)
            },
            new()
            {
                Id = 100014,
                Name = "Rocket League",
                Background_Image =
                    "https://cdn1.epicgames.com/offer/9773aa1aa54f4f7b80e44bef04986cea/EGS_RocketLeague_PsyonixLLC_S1_2560x1440-1a37e26b20fb4f3ebd825e64bc7914eb",
                Metacritic = 86,
                Platforms = On(Pc, Ps4, XboxOne, Switch)
            },
            new()
            {
                Id = 100015,
                Name = "Forza Horizon 5",
                Background_Image =
                    "https://image.api.playstation.com/vulcan/ap/rnd/202501/2717/42b3ee6b1b2094212231b0b0a82824f687fc5c4dc9bde31c.png",
                Metacritic = 92,
                Platforms = On(Pc, XboxSeries, XboxOne)
            },
            new()
            {
                Id = 100016,
                Name = "God of War",
                Background_Image =
                    "https://www.memorypc.de/media/13/4c/35/1751618004/God_of_War-Spin-off_wohl_erst_2026Insider_berichten_von_Verzogerung.jpg?ts=1751618004",
                Metacritic = 94,
                Platforms = On(Pc, Ps4, Ps5)
            },
            new()
            {
                Id = 100017,
                Name = "Animal Crossing: New Horizons",
                Background_Image =
                    "https://media.wired.com/photos/5fa5be20daa25f804cdbd2d9/4:3/w_1440,h_1080,c_limit/games_culture_anch-fall.jpg",
                Metacritic = 90,
                Platforms = On(Switch)
            },
            new()
            {
                Id = 100018,
                Name = "Cyberpunk 2077",
                Background_Image = "https://www.excal.on.ca/wp-content/uploads/2021/02/image3.png",
                Metacritic = 86,
                Platforms = On(Pc, Ps5, XboxSeries)
            },
            new()
            {
                Id = 100019,
                Name = "Dead Cells",
                Background_Image =
                    "https://cdn.wccftech.com/wp-content/uploads/2018/08/Dead-Cells-Key-Art.jpg",
                Metacritic = 89,
                Platforms = On(Pc, Switch, Ps4, XboxOne)
            },
            new()
            {
                Id = 100020,
                Name = "FTL: Faster Than Light",
                Background_Image =
                    "https://www.hd-tecnologia.com/imagenes/articulos/2019/12/FTL-Faster-Than-Light-ya-est%C3%A1-gratis-en-la-Epic-Store.jpg",
                Metacritic = 84,
                Platforms = On(Pc, MacOs, Linux, Ios)
            },
            new()
            {
                Id = 100021,
                Name = "It Takes Two",
                Background_Image =
                    "https://assets.nintendo.com/image/upload/c_fill,w_1200/q_auto:best/f_auto/dpr_2.0/store/software/switch/70010000049281/e7200824041808289d4a65589ed368f7e08dc2e538a5fd7ee9f8d39e58015c24",
                Metacritic = 89,
                Platforms = On(Pc, Ps5, Ps4, XboxSeries, XboxOne, Switch)
            },
            new()
            {
                Id = 100022,
                Name = "Terraria",
                Background_Image =
                    "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/105600/capsule_616x353.jpg?t=1769844435",
                Metacritic = 83,
                Platforms = On(Pc, Switch, Ps4, XboxOne, Ios, Android)
            },
            new()
            {
                Id = 100023,
                Name = "Among Us",
                Background_Image =
                    "https://assets.nintendo.com/image/upload/c_fill,w_1200/q_auto:best/f_auto/dpr_2.0/store/software/switch/70010000036098/758ab0b61205081da2466386940752c70e0e5ea43bd39e8b9b13eaa455c69b7e",
                Metacritic = 85,
                Platforms = On(Pc, Switch, Ios, Android)
            },
            new()
            {
                Id = 100024,
                Name = "Ori and the Blind Forest",
                Background_Image =
                    "https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/261570/capsule_616x353.jpg?t=1667504148",
                Metacritic = 88,
                Platforms = On(Pc, Switch, XboxOne)
            },
            new()
            {
                Id = 100025,
                Name = "Persona 5 Royal",
                Background_Image =
                    "https://assets.nintendo.com/image/upload/c_fill,w_1200/q_auto:best/f_auto/dpr_2.0/store/software/switch/70010000043147/684bd8b00abcbf6dd122727a27c01a337f667bef825f4f4662efad9854b72fd4",
                Metacritic = 95,
                Platforms = On(Pc, Ps4, Switch, XboxOne)
            },
            new()
            {
                Id = 100026,
                Name = "NBA 2K24",
                Background_Image =
                    "https://i.blogs.es/34331d/ss_ba564133aeb8b8c6433f7e65c340faf4823069e4/450_1000.jpeg",
                Metacritic = 66,
                Platforms = On(Pc, Ps5, XboxSeries, Switch)
            },
            new()
            {
                Id = 100027,
                Name = "Mario Kart 8 Deluxe",
                Background_Image =
                    "https://sm.ign.com/ign_latam/review/m/mario-kart/mario-kart-8-deluxe-review_vg8p.jpg",
                Metacritic = 92,
                Platforms = On(Switch)
            },
            new()
            {
                Id = 100028,
                Name = "Disco Elysium",
                Background_Image =
                    "https://indiehoy.com/wp-content/uploads/2024/12/Disco-Elysium.jpg",
                Metacritic = 91,
                Platforms = On(Pc, Ps4, XboxOne, Switch)
            },
        };

    // RawgGameDto has no genre field, so genre membership for the fallback dataset
    // is tracked separately here and only consumed by RawgApiService's fallback filtering.
    public static readonly Dictionary<int, List<int>> GameGenreIds =
        new()
        {
            [100001] = new() { Rpg, Adventure },
            [100002] = new() { Action, Indie, Rpg },
            [100003] = new() { Simulation, Casual, Indie },
            [100004] = new() { Shooter, Strategy },
            [100005] = new() { Rpg, Action, Adventure },
            [100006] = new() { Indie, Adventure },
            [100007] = new() { Action, Adventure, Indie },
            [100008] = new() { Rpg, Strategy },
            [100009] = new() { Simulation, Adventure, Casual },
            [100010] = new() { Shooter, Action },
            [100011] = new() { Strategy, Indie, Casual },
            [100012] = new() { Strategy, Simulation },
            [100013] = new() { Shooter, Action },
            [100014] = new() { Sports, Action, Casual },
            [100015] = new() { Racing, Simulation },
            [100016] = new() { Action, Adventure },
            [100017] = new() { Simulation, Casual },
            [100018] = new() { Rpg, Shooter, Action },
            [100019] = new() { Action, Indie },
            [100020] = new() { Strategy, Indie, Simulation },
            [100021] = new() { Adventure, Action, Indie },
            [100022] = new() { Simulation, Adventure, Indie },
            [100023] = new() { Casual, Indie },
            [100024] = new() { Adventure, Indie },
            [100025] = new() { Rpg, Adventure },
            [100026] = new() { Sports, Simulation },
            [100027] = new() { Racing, Casual },
            [100028] = new() { Rpg, Adventure, Indie },
        };
}
