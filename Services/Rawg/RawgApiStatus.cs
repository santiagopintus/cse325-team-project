namespace QuestLog.Services.Rawg;

// App-wide (singleton) circuit breaker: once RAWG proves unreachable, every subsequent call
// (search, popular games, "see more", genres, platforms) skips the network entirely and goes
// straight to the local fallback dataset for the rest of the process's lifetime. RAWG can be
// down for extended periods, so we only probe it once per app run rather than re-checking every
// few minutes - restart the app to re-check.
public class RawgApiStatus
{
    private readonly object _lock = new();

    public bool IsDown { get; private set; }

    public event Action? Changed;

    public void MarkDown()
    {
        lock (_lock)
        {
            if (IsDown) return;
            IsDown = true;
        }

        Changed?.Invoke();
    }
}
