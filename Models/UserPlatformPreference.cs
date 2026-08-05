namespace QuestLog.Models;

public class UserPlatformPreference
{
    public int Id { get; set; }

    public string UserId { get; set; } = string.Empty;

    // RAWG platform id
    public int PlatformId { get; set; }
    public string PlatformName { get; set; } = string.Empty;
}
