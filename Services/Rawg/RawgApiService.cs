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

    public async Task<RawgPagedResult> SearchGamesAsync(
        string query,
        int page = 1,
        int pageSize = 20,
        List<int>? genreIds = null,
        List<int>? platformIds = null,
        int? minMetacritic = null)
    {
        if (string.IsNullOrWhiteSpace(query)) return new();

        var url = $"https://api.rawg.io/api/games?key={_apiKey}&search={Uri.EscapeDataString(query)}&page={page}&page_size={pageSize}";
        url += BuildFilterQuery(genreIds, platformIds, minMetacritic);
        var result = await FetchAsync(url);

        _logger.LogInformation(
            "RAWG search for \"{Query}\" (page {Page}) returned {Count} games: {Titles}",
            query,
            page,
            result.Games.Count,
            string.Join(", ", result.Games.Take(5).Select(g => g.Name)));

        return result;
    }

    // "-added" orders by how many users have added the game on RAWG, i.e. its most popular titles.
    public async Task<RawgPagedResult> GetPopularGamesAsync(
        int page = 1,
        int pageSize = 20,
        List<int>? genreIds = null,
        List<int>? platformIds = null,
        int? minMetacritic = null)
    {
        var url = $"https://api.rawg.io/api/games?key={_apiKey}&ordering=-added&page={page}&page_size={pageSize}";
        url += BuildFilterQuery(genreIds, platformIds, minMetacritic);
        var result = await FetchAsync(url);

        _logger.LogInformation(
            "RAWG popular games fetch (page {Page}) returned {Count} games: {Titles}",
            page,
            result.Games.Count,
            string.Join(", ", result.Games.Take(5).Select(g => g.Name)));

        return result;
    }

    public async Task<List<RawgGenreDto>> GetGenresAsync()
    {
        var url = $"https://api.rawg.io/api/genres?key={_apiKey}&page_size=40";
        var response = await _httpClient.GetFromJsonAsync<RawgGenreListResponse>(url);
        return response?.Results ?? new();
    }

    public async Task<List<RawgPlatformDto>> GetPlatformsAsync()
    {
        var url = $"https://api.rawg.io/api/platforms?key={_apiKey}&page_size=100";
        var response = await _httpClient.GetFromJsonAsync<RawgPlatformListResponse>(url);
        return response?.Results ?? new();
    }

    private static string BuildFilterQuery(List<int>? genreIds, List<int>? platformIds, int? minMetacritic)
    {
        var query = "";

        if (genreIds is { Count: > 0 })
        {
            query += $"&genres={string.Join(",", genreIds)}";
        }

        if (platformIds is { Count: > 0 })
        {
            query += $"&platforms={string.Join(",", platformIds)}";
        }

        if (minMetacritic is > 0)
        {
            query += $"&metacritic={minMetacritic},100";
        }

        return query;
    }

    private async Task<RawgPagedResult> FetchAsync(string url)
    {
        var response = await _httpClient.GetFromJsonAsync<RawgResponse>(url);
        return new RawgPagedResult
        {
            Games = response?.Results ?? new(),
            HasMore = !string.IsNullOrEmpty(response?.Next)
        };
    }
}
