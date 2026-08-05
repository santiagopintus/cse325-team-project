using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuestLog.Models;

namespace QuestLog.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<UserGame> UserGames { get; set; }
    public DbSet<UserPlatformPreference> UserPlatformPreferences { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Ensure a user cannot add the same RAWG game twice
        builder.Entity<UserGame>()
            .HasIndex(g => new { g.UserId, g.RawgId })
            .IsUnique();

        // Ensure a user cannot save the same preferred platform twice
        builder.Entity<UserPlatformPreference>()
            .HasIndex(p => new { p.UserId, p.PlatformId })
            .IsUnique();
    }
}
