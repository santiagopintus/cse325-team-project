namespace QuestLog.Services.Rawg;

public class RawgResponse
{
    public List<RawgGameDto> Results { get; set; } = new();
}

public class RawgGameDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Background_Image { get; set; } = string.Empty;
    public List<PlatformContainerDto> Platforms { get; set; } = new();
}

public class PlatformContainerDto
{
    public PlatformDto Platform { get; set; } = new();
}

public class PlatformDto
{
    public string Name { get; set; } = string.Empty;
}
