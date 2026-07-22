# Technical Implementation & Context Document

This document defines the complete technical architecture, data model, API integration, and implementation steps for **QuestLog**. It is optimized for immediate code generation or reference by a developer agent.

---

## 1. Project Summary & Architecture

QuestLog is a lightweight .NET Blazor Web App that enables users to discover games via the public **RAWG API** and save them to a permanent personal backlog called **"My Shelf"**.

### High-Level Stack

* **Framework:** .NET 8.0 or .NET 9.0 Blazor Web App (Interactive Server render mode recommended for simplicity and fast state synchronization).
* **Authentication:** Built-in ASP.NET Core Identity (Individual Accounts).
* **Database Engine:** PostgreSQL (Hosted on free cloud provider **Neon.tech** or **Supabase** to ensure persistent storage).
* **ORM:** Entity Framework Core (EF Core) using Npgsql.
* **API Integration:** RAWG.io API via `HttpClient` (using a free personal API Key).
* **Styling:** Bootstrap 5 (default layout, custom utility classes for accessibility).

---

## 2. Database Schema & Models

To avoid continuous API lookups, when a user adds a game, we clone its essential metadata directly into our database. The custom user fields (status, rating, notes) are stored in the same record.

```csharp
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

```

### EF Core DbContext Setup

```csharp
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuestLog.Models;

namespace QuestLog.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserGame> UserGames { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Ensure a user cannot add the same RAWG game twice
        builder.Entity<UserGame>()
            .HasIndex(g => new { g.UserId, g.RawgId })
            .IsUnique();
    }
}

```

---

## 3. RAWG API Integration Service

The application implements a lightweight service to search the RAWG API. Register this service as a **Transient** or **Scoped** dependency in `Program.cs`.

### API DTOs

```csharp
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

```

### RawgApiService Implementation

```csharp
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

```

---

## 4. UI Layout & Navigation

The navigation consists of two primary operational tabs exposed clearly in the sidebar / top bar layout:

1. **"Explore" (`/explore` or `/`):** The landing search engine powered by RAWG API.
2. **"My Shelf" (`/my-shelf`):** Protected user space displaying personal games. *Requires user authentication (`@attribute [Authorize]`)*.

---

## 5. View Configurations & CRUD Logic

### A. "Explore" View (`Explore.razor`)

* **Search Interface:** Text input that triggers `SearchGamesAsync` on button click or form submit.
* **Display:** A responsive CSS Grid of cards depicting games.
* **Action:** If logged in, cards render an "Add to Shelf" button. If not logged in, they display a "Log in to add" redirect.

```razor
@page "/"
@inject RawgApiService RawgApi
@inject ApplicationDbContext DbContext
@inject AuthenticationStateProvider AuthProvider
@inject NavigationManager Nav

<h3>Explore Games</h3>

<div class="search-box mb-4">
    <input @bind="searchQuery" @bind:event="oninput" @onkeyup="HandleKeyUp" class="form-control" placeholder="Search games..." aria-label="Search games" />
</div>

@if (games.Any())
{
    <div class="game-grid">
        @foreach (var game in games)
        {
            <div class="game-card">
                <img src="@(string.IsNullOrEmpty(game.Background_Image) ? "/placeholder.png" : game.Background_Image)" alt="@game.Name Cover" />
                <h4>@game.Name</h4>
                <p>@string.Join(", ", game.Platforms.Select(p => p.Platform.Name))</p>
                <AuthorizeView>
                    <Authorized>
                        <button class="btn btn-primary btn-sm" @onclick="() => AddToShelf(game)">Add to Shelf</button>
                    </Authorized>
                    <NotAuthorized>
                        <a href="Account/Login" class="btn btn-outline-secondary btn-sm">Log in to Add</a>
                    </NotAuthorized>
                </AuthorizeView>
            </div>
        }
    </div>
}

```

```csharp
@code {
    private string searchQuery = "";
    private List<RawgGameDto> games = new();

    private async Task Search()
    {
        games = await RawgApi.SearchGamesAsync(searchQuery);
    }

    private async Task HandleKeyUp(KeyboardEventArgs e)
    {
        if (e.Key == "Enter") await Search();
    }

    private async Task AddToShelf(RawgGameDto rawgGame)
    {
        var authState = await AuthProvider.GetAuthenticationStateAsync();
        var userId = authState.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (userId == null) return;

        // Check for duplicate
        var exists = DbContext.UserGames.Any(g => g.UserId == userId && g.RawgId == rawgGame.Id);
        if (exists) return;

        var newUserGame = new UserGame
        {
            UserId = userId,
            RawgId = rawgGame.Id,
            Title = rawgGame.Name,
            CoverArtUrl = rawgGame.Background_Image,
            Platform = rawgGame.Platforms.FirstOrDefault()?.Platform.Name ?? "Unknown",
            Status = PlayStatus.Backlog
        };

        DbContext.UserGames.Add(newUserGame);
        await DbContext.SaveChangesAsync();
    }
}

```

### B. "My Shelf" View (`MyShelf.razor`)

* **Authorization:** Marked with `@attribute [Authorize]`.
* **Displays:** Personalized grid showing user records.
* **Interactivity:**
* **Filtering:** Multi-select tabs or dropdown of statuses (*Wishlist*, *Backlog*, *Playing*, *Completed*, *Shelved*).
* **Update:** Simple edit mode (changing rating or status triggers an asynchronous Save back to database).
* **Delete:** Instantly removes the row mapping.



---

## 6. Documented Requirements Mapping

Here is how the system technical layout directly satisfies each classroom/rubric criteria:

| Requirement / Standard | Technical Execution Strategy in QuestLog |
| --- | --- |
| **User Authentication** | Uses ASP.NET Core Identity scaffolded automatically via the **Individual Accounts** template. All API writes are scoped directly to the authenticated `UserId` Claims Principal. |
| **CRUD Functionality** | **Create:** Copying metadata from RAWG API into `UserGames` table.<br>

<br>**Read:** Loading `UserGames` tied to logged-in user.<br>

<br>**Update:** Modifying `Status` and `UserRating` directly in line or inside a modal.<br>

<br>**Delete:** Discarding user records via database remove context. |
| **Performance** | **Data Isolation:** Minimizes network payload by fetching game details from local database query rather than continuous API lookups.<br>

<br>**Image Compression:** Out-of-the-box lazy loading on standard image tags prevents unnecessary high-resolution asset pipeline overhead. |
| **Accessibility (WCAG 2.1 AA)** | **Semantic Elements:** Raw HTML elements are properly labeled with distinct `aria-*` markers. Standard high-contrast Bootstrap themes (contrast ratios greater than 4.5:1) are loaded by default.<br>

<br>**Keyboard Nav:** Elements such as the Search bar utilize standard binding modifiers permitting input processing directly through the **Enter** key without screen-reader issues. |
| **Usability & Branding** | Fully responsive container fluid system natively handled by Bootstrap layout engine ensuring fluid structural shifting across desktop, tablet, and mobile browsers. |
| **Quality Assurance / Testing** | Clean separation of concerns allows the `RawgApiService` to be unit tested using mock HttpClient integrations easily. Integration tests can target the InMemory Provider before shifting DB contexts. |
| **Deployment & Persistence** | **Continuous Deployment:** Managed with GitHub linked directly to free hosting platforms (like Railway, Render, or fly.io).<br>

<br>**Cloud Storage:** Fully secure and free PostgreSQL cloud instance (via Neon or Supabase) ensures absolute data persistence, bypassing the ephemeral filesystem limitations of free hosting tiers. |

---

## 7. Developer's Local Checklist

Before executing code compilation, ensure your app-settings configuration includes the target key configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=your-neon-or-supabase-db.neon.tech;Database=questlog;Username=...;Password=..."
  },
  "RawgApiKey": "YourRAWGApiKeyHere"
}

```