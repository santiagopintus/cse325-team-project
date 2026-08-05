namespace QuestLog.Services.Rawg;

public class ExploreGamesCache
{
    public bool IsPopulated { get; set; }
    public string? Query { get; set; }
    public string? FilterSignature { get; set; }
    public List<RawgGameDto> Games { get; set; } = new();
    public HashSet<int> AddedRawgIds { get; set; } = new();
    public bool HasMore { get; set; }
    public int CurrentPage { get; set; } = 1;
    public bool HasSearched { get; set; }
    public bool IsFallbackActive { get; set; }
}
