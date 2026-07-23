using System.Net.Http.Json;

namespace QuestLog.Services.Rawg;

public class RawgApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly ILogger<RawgApiService> _logger;

    public RawgApiService(HttpClient httpClient, IConfiguration configuration, ILogger<RawgApiService> logger)
    {
        _httpClient = httpClient;
        _apiKey = configuration["RawgApiKey"] ?? throw new InvalidOperationException("RAWG API key is missing.");
        _logger = logger;
    }

    public async Task<List<RawgGameDto>> SearchGamesAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return new();

        var url = $"https://api.rawg.io/api/games?key={_apiKey}&search={Uri.EscapeDataString(query)}&page_size=20";
        var games = await FetchAsync(url);

        _logger.LogInformation(
            "RAWG search for \"{Query}\" returned {Count} games: {Titles}",
            query,
            games.Count,
            string.Join(", ", games.Take(5).Select(g => g.Name)));

        return games;
    }

    // "-added" orders by how many users have added the game on RAWG, i.e. its most popular titles.
    public async Task<List<RawgGameDto>> GetPopularGamesAsync(int pageSize = 20)
    {
        var url = $"https://api.rawg.io/api/games?key={_apiKey}&ordering=-added&page_size={pageSize}";
        var games = await FetchAsync(url);

        _logger.LogInformation(
            "RAWG popular games fetch returned {Count} games: {Titles}",
            games.Count,
            string.Join(", ", games.Take(5).Select(g => g.Name)));

        return games;
    }

    private async Task<List<RawgGameDto>> FetchAsync(string url)
    {
        var response = await _httpClient.GetFromJsonAsync<RawgResponse>(url);
        return response?.Results ?? new();
    }
}
