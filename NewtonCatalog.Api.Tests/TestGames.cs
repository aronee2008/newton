using NewtonCatalog.Api.Generated;

namespace NewtonCatalog.Api.Tests;

public static class TestGames
{
    public static Game Hades() => new()
    {
        Id = 1,
        Title = "Hades",
        Developer = "Supergiant Games",
        Platform = Platform.Pc,
        Genre = Genre.Action,
        ReleaseDate = new DateOnly(2020, 9, 17),
        Rating = 93,
        Price = 24.99m
    };

    public static Game Celeste() => new()
    {
        Id = 2,
        Title = "Celeste",
        Developer = "Maddy Makes Games",
        Platform = Platform.NintendoSwitch,
        Genre = Genre.Platformer,
        ReleaseDate = new DateOnly(2018, 1, 25),
        Rating = 92,
        Price = 19.99m
    };

    public static UpdateGameRequest ValidUpdate() => new()
    {
        Title = "Hades II",
        Developer = "Supergiant Games",
        Platform = Platform.PlayStation5,
        Genre = Genre.Rpg,
        ReleaseDate = new DateOnly(2024, 5, 6),
        Rating = 90,
        Price = 29.99m
    };
}
