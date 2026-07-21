namespace QuestLog.Models;

public enum PlayStatus
{
    Wishlist,
    Backlog,
    Playing,
    Completed,
    Shelved
}

public class UserGame
{
    public int Id { get; set; }

    // Identity Link
    public string UserId { get; set; } = string.Empty;

    // RAWG API Source ID (to prevent duplicate additions)
    public int RawgId { get; set; }

    // Cloned Metadata
    public string Title { get; set; } = string.Empty;
    public string CoverArtUrl { get; set; } = string.Empty;
    public string Platform { get; set; } = string.Empty; // Primary platform from RAWG

    // User Custom Data (CRUD target attributes)
    public PlayStatus Status { get; set; } = PlayStatus.Backlog;
    public int UserRating { get; set; } = 0; // 0 to 5 stars
    public string Notes { get; set; } = string.Empty;

    public DateTime DateAdded { get; set; } = DateTime.UtcNow;
    public DateTime? DateUpdated { get; set; }
}
