namespace QuestLog.Services.Rawg;

// Shared in-memory cache for RAWG's genres/platforms lists, registered as a singleton so it is
// shared across the prerender request and the interactive circuit (which otherwise run in
// separate DI scopes and each pay RAWG's request timeout independently), as well as across
// different pages and users. Keeps every page load from re-waiting on RAWG's timeout while it's
// down, whether the cached value is live data or the local fallback dataset.
public class RawgReferenceDataCache
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(2);
    private readonly object _lock = new();

    private List<RawgGenreDto>? _genres;
    private DateTime _genresCachedAt;
    private bool _genresIsFallback;

    private List<RawgPlatformDto>? _platforms;
    private DateTime _platformsCachedAt;
    private bool _platformsIsFallback;

    public bool TryGetGenres(out List<RawgGenreDto> genres, out bool isFallback)
    {
        lock (_lock)
        {
            if (_genres is not null && DateTime.UtcNow - _genresCachedAt < CacheDuration)
            {
                genres = _genres;
                isFallback = _genresIsFallback;
                return true;
            }
        }

        genres = new();
        isFallback = false;
        return false;
    }

    public void SetGenres(List<RawgGenreDto> genres, bool isFallback)
    {
        lock (_lock)
        {
            _genres = genres;
            _genresCachedAt = DateTime.UtcNow;
            _genresIsFallback = isFallback;
        }
    }

    public bool TryGetPlatforms(out List<RawgPlatformDto> platforms, out bool isFallback)
    {
        lock (_lock)
        {
            if (_platforms is not null && DateTime.UtcNow - _platformsCachedAt < CacheDuration)
            {
                platforms = _platforms;
                isFallback = _platformsIsFallback;
                return true;
            }
        }

        platforms = new();
        isFallback = false;
        return false;
    }

    public void SetPlatforms(List<RawgPlatformDto> platforms, bool isFallback)
    {
        lock (_lock)
        {
            _platforms = platforms;
            _platformsCachedAt = DateTime.UtcNow;
            _platformsIsFallback = isFallback;
        }
    }
}
