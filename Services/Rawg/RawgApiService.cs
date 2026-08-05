using System.Net.Http.Json;

namespace QuestLog.Services.Rawg;

public class RawgApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly ILogger<RawgApiService> _logger;
    private readonly RawgReferenceDataCache _referenceCache;
    private readonly RawgApiStatus _status;

    public RawgApiService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<RawgApiService> logger,
        RawgReferenceDataCache referenceCache,
        RawgApiStatus status)
    {
        _httpClient = httpClient;
        _apiKey = configuration["RawgApiKey"] ?? throw new InvalidOperationException("RAWG API key is missing.");
        _logger = logger;
        _referenceCache = referenceCache;
        _status = status;
    }

    // True when the most recent fetch (games, genres, or platforms) had to fall back to
    // the local offline dataset because RAWG was unreachable.
    public bool LastFetchWasFallback { get; private set; }

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
        var result = await FetchOrFallbackAsync(url, query, page, pageSize, genreIds, platformIds, minMetacritic);

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
        var result = await FetchOrFallbackAsync(url, null, page, pageSize, genreIds, platformIds, minMetacritic);

        _logger.LogInformation(
            "RAWG popular games fetch (page {Page}) returned {Count} games: {Titles}",
            page,
            result.Games.Count,
            string.Join(", ", result.Games.Take(5).Select(g => g.Name)));

        return result;
    }

    public async Task<List<RawgGenreDto>> GetGenresAsync()
    {
        if (_status.IsDown)
        {
            LastFetchWasFallback = true;
            return RawgFallbackData.Genres;
        }

        if (_referenceCache.TryGetGenres(out var cachedGenres, out var cachedIsFallback))
        {
            LastFetchWasFallback = cachedIsFallback;
            return cachedGenres;
        }

        var url = $"https://api.rawg.io/api/genres?key={_apiKey}&page_size=40";

        try
        {
            var response = await _httpClient.GetFromJsonAsync<RawgGenreListResponse>(url);
            var genres = response?.Results ?? new();
            LastFetchWasFallback = false;
            _referenceCache.SetGenres(genres, isFallback: false);
            return genres;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to fetch RAWG genres; falling back to local demo dataset.");
            _status.MarkDown();
            LastFetchWasFallback = true;
            _referenceCache.SetGenres(RawgFallbackData.Genres, isFallback: true);
            return RawgFallbackData.Genres;
        }
    }

    public async Task<List<RawgPlatformDto>> GetPlatformsAsync()
    {
        if (_status.IsDown)
        {
            LastFetchWasFallback = true;
            return RawgFallbackData.Platforms;
        }

        if (_referenceCache.TryGetPlatforms(out var cachedPlatforms, out var cachedIsFallback))
        {
            LastFetchWasFallback = cachedIsFallback;
            return cachedPlatforms;
        }

        var url = $"https://api.rawg.io/api/platforms?key={_apiKey}&page_size=100";

        try
        {
            var response = await _httpClient.GetFromJsonAsync<RawgPlatformListResponse>(url);
            var platforms = response?.Results ?? new();
            LastFetchWasFallback = false;
            _referenceCache.SetPlatforms(platforms, isFallback: false);
            return platforms;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to fetch RAWG platforms; falling back to local demo dataset.");
            _status.MarkDown();
            LastFetchWasFallback = true;
            _referenceCache.SetPlatforms(RawgFallbackData.Platforms, isFallback: true);
            return RawgFallbackData.Platforms;
        }
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

    private async Task<RawgPagedResult> FetchOrFallbackAsync(
        string url,
        string? nameFilter,
        int page,
        int pageSize,
        List<int>? genreIds,
        List<int>? platformIds,
        int? minMetacritic)
    {
        if (_status.IsDown)
        {
            LastFetchWasFallback = true;
            return BuildFallbackPagedResult(nameFilter, page, pageSize, genreIds, platformIds, minMetacritic);
        }

        try
        {
            var result = await FetchAsync(url);
            LastFetchWasFallback = false;
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "RAWG game fetch failed; falling back to local demo dataset.");
            _status.MarkDown();
            LastFetchWasFallback = true;
            return BuildFallbackPagedResult(nameFilter, page, pageSize, genreIds, platformIds, minMetacritic);
        }
    }

    private static RawgPagedResult BuildFallbackPagedResult(
        string? query,
        int page,
        int pageSize,
        List<int>? genreIds,
        List<int>? platformIds,
        int? minMetacritic)
    {
        var filtered = RawgFallbackData.Games.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            filtered = filtered.Where(g => g.Name.Contains(query, StringComparison.OrdinalIgnoreCase));
        }

        if (genreIds is { Count: > 0 })
        {
            filtered = filtered.Where(g =>
                RawgFallbackData.GameGenreIds.TryGetValue(g.Id, out var ids) && ids.Any(genreIds.Contains));
        }

        if (platformIds is { Count: > 0 })
        {
            var platformNames = RawgFallbackData.Platforms
                .Where(p => platformIds.Contains(p.Id))
                .Select(p => p.Name)
                .ToHashSet();
            filtered = filtered.Where(g => g.Platforms.Any(pc => platformNames.Contains(pc.Platform.Name)));
        }

        if (minMetacritic is > 0)
        {
            filtered = filtered.Where(g => g.Metacritic >= minMetacritic);
        }

        var list = filtered.ToList();
        var pageItems = list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        var hasMore = page * pageSize < list.Count;

        return new RawgPagedResult { Games = pageItems, HasMore = hasMore };
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
