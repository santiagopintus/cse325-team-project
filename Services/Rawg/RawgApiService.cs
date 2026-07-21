using System.Net.Http.Json;

namespace QuestLog.Services.Rawg;

public class RawgApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public RawgApiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["RawgApiKey"] ?? throw new InvalidOperationException("RAWG API key is missing.");
    }

    public async Task<List<RawgGameDto>> SearchGamesAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return new();

        var url = $"https://api.rawg.io/api/games?key={_apiKey}&search={Uri.EscapeDataString(query)}&page_size=20";
        var response = await _httpClient.GetFromJsonAsync<RawgResponse>(url);

        return response?.Results ?? new();
    }
}
