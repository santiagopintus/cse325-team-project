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
                Background_Image = Cover("Witcher 3", "#6b2d5c"),
                Metacritic = 92,
                Platforms = On(Pc, Ps4, XboxOne, Switch)
            },
            new()
            {
                Id = 100002,
                Name = "Hades",
                Background_Image = Cover("Hades", "#8c1c1c"),
                Metacritic = 93,
                Platforms = On(Pc, Ps5, Switch, MacOs)
            },
            new()
            {
                Id = 100003,
                Name = "Stardew Valley",
                Background_Image = Cover("Stardew Valley", "#4a7c3a"),
                Metacritic = 89,
                Platforms = On(Pc, Switch, Ios, Android)
            },
            new()
            {
                Id = 100004,
                Name = "Portal 2",
                Background_Image = Cover("Portal 2", "#2a6f6f"),
                Metacritic = 95,
                Platforms = On(Pc, MacOs, Linux)
            },
            new()
            {
                Id = 100005,
                Name = "Elden Ring",
                Background_Image = Cover("Elden Ring", "#4a3b1c"),
                Metacritic = 96,
                Platforms = On(Pc, Ps5, XboxSeries)
            },
            new()
            {
                Id = 100006,
                Name = "Celeste",
                Background_Image = Cover("Celeste", "#3a4a8c"),
                Metacritic = 92,
                Platforms = On(Pc, Switch, Ps4, XboxOne)
            },
            new()
            {
                Id = 100007,
                Name = "Hollow Knight",
                Background_Image = Cover("Hollow Knight", "#2c2c3a"),
                Metacritic = 90,
                Platforms = On(Pc, Switch, Ps4, XboxOne)
            },
            new()
            {
                Id = 100008,
                Name = "Baldur's Gate 3",
                Background_Image = Cover("Baldur's Gate 3", "#7c1c1c"),
                Metacritic = 96,
                Platforms = On(Pc, Ps5, XboxSeries)
            },
            new()
            {
                Id = 100009,
                Name = "Minecraft",
                Background_Image = Cover("Minecraft", "#3a7c3a"),
                Metacritic = 93,
                Platforms = On(Pc, Ps4, XboxOne, Switch, Ios, Android)
            },
            new()
            {
                Id = 100010,
                Name = "Half-Life 2",
                Background_Image = Cover("Half-Life 2", "#5c5c5c"),
                Metacritic = 96,
                Platforms = On(Pc, MacOs, Linux)
            },
            new()
            {
                Id = 100011,
                Name = "Slay the Spire",
                Background_Image = Cover("Slay the Spire", "#7c3a8c"),
                Metacritic = 87,
                Platforms = On(Pc, Switch, Ps4, Ios)
            },
            new()
            {
                Id = 100012,
                Name = "Civilization VI",
                Background_Image = Cover("Civilization VI", "#1c5c8c"),
                Metacritic = 88,
                Platforms = On(Pc, Switch, XboxOne, Ios)
            },
            new()
            {
                Id = 100013,
                Name = "Fortnite",
                Background_Image = Cover("Fortnite", "#5c1c8c"),
                Metacritic = 78,
                Platforms = On(Pc, Ps5, XboxSeries, Switch, Ios, Android)
            },
            new()
            {
                Id = 100014,
                Name = "Rocket League",
                Background_Image = Cover("Rocket League", "#1c3a8c"),
                Metacritic = 86,
                Platforms = On(Pc, Ps4, XboxOne, Switch)
            },
            new()
            {
                Id = 100015,
                Name = "Forza Horizon 5",
                Background_Image = Cover("Forza Horizon 5", "#8c5c1c"),
                Metacritic = 92,
                Platforms = On(Pc, XboxSeries, XboxOne)
            },
            new()
            {
                Id = 100016,
                Name = "God of War",
                Background_Image = Cover("God of War", "#3a3a3a"),
                Metacritic = 94,
                Platforms = On(Pc, Ps4, Ps5)
            },
            new()
            {
                Id = 100017,
                Name = "Animal Crossing: New Horizons",
                Background_Image = Cover("Animal Crossing", "#7ac48c"),
                Metacritic = 90,
                Platforms = On(Switch)
            },
            new()
            {
                Id = 100018,
                Name = "Cyberpunk 2077",
                Background_Image = Cover("Cyberpunk 2077", "#8c1c5c"),
                Metacritic = 86,
                Platforms = On(Pc, Ps5, XboxSeries)
            },
            new()
            {
                Id = 100019,
                Name = "Dead Cells",
                Background_Image = Cover("Dead Cells", "#8c1c1c"),
                Metacritic = 89,
                Platforms = On(Pc, Switch, Ps4, XboxOne)
            },
            new()
            {
                Id = 100020,
                Name = "FTL: Faster Than Light",
                Background_Image = Cover("FTL", "#1c1c5c"),
                Metacritic = 84,
                Platforms = On(Pc, MacOs, Linux, Ios)
            },
            new()
            {
                Id = 100021,
                Name = "It Takes Two",
                Background_Image = Cover("It Takes Two", "#8c6c1c"),
                Metacritic = 89,
                Platforms = On(Pc, Ps5, Ps4, XboxSeries, XboxOne, Switch)
            },
            new()
            {
                Id = 100022,
                Name = "Terraria",
                Background_Image = Cover("Terraria", "#3a8c5c"),
                Metacritic = 83,
                Platforms = On(Pc, Switch, Ps4, XboxOne, Ios, Android)
            },
            new()
            {
                Id = 100023,
                Name = "Among Us",
                Background_Image = Cover("Among Us", "#c43a3a"),
                Metacritic = 85,
                Platforms = On(Pc, Switch, Ios, Android)
            },
            new()
            {
                Id = 100024,
                Name = "Ori and the Blind Forest",
                Background_Image = Cover("Ori", "#1c5c5c"),
                Metacritic = 88,
                Platforms = On(Pc, Switch, XboxOne)
            },
            new()
            {
                Id = 100025,
                Name = "Persona 5 Royal",
                Background_Image = Cover("Persona 5", "#8c1c1c"),
                Metacritic = 95,
                Platforms = On(Pc, Ps4, Switch, XboxOne)
            },
            new()
            {
                Id = 100026,
                Name = "NBA 2K24",
                Background_Image = Cover("NBA 2K24", "#8c3a1c"),
                Metacritic = 66,
                Platforms = On(Pc, Ps5, XboxSeries, Switch)
            },
            new()
            {
                Id = 100027,
                Name = "Mario Kart 8 Deluxe",
                Background_Image = Cover("Mario Kart 8", "#c43a3a"),
                Metacritic = 92,
                Platforms = On(Switch)
            },
            new()
            {
                Id = 100028,
                Name = "Disco Elysium",
                Background_Image = Cover("Disco Elysium", "#3a3a8c"),
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
