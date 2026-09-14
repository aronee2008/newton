using Microsoft.EntityFrameworkCore;
using NewtonCatalog.Api.Data;
using NewtonCatalog.Api.Generated;
using NewtonCatalog.Api.Repositories;
using NewtonCatalog.Api.Services;

namespace NewtonCatalog.Api.Tests.Services;

public class VideoGameServiceTests : IAsyncLifetime
{
    private readonly CatalogueDbContext _db = new(
        new DbContextOptionsBuilder<CatalogueDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private VideoGameService _service = null!;

    public async Task InitializeAsync()
    {
        _db.Games.AddRange(TestGames.Hades(), TestGames.Celeste());
        await _db.SaveChangesAsync();
        _db.ChangeTracker.Clear();

        _service = new VideoGameService(new VideoGameRepository(_db));
    }

    public Task DisposeAsync() => _db.DisposeAsync().AsTask();

    [Fact]
    public async Task GetAll_ReturnsGamesOrderedByTitle()
    {
        var games = await _service.GetAllAsync(CancellationToken.None);

        Assert.Equal(["Celeste", "Hades"], games.Select(g => g.Title));
    }

    [Fact]
    public async Task GetById_UnknownId_ReturnsNull()
    {
        var game = await _service.GetByIdAsync(999, CancellationToken.None);

        Assert.Null(game);
    }

    [Fact]
    public async Task Update_PersistsChanges()
    {
        var request = TestGames.ValidUpdate();

        var updated = await _service.UpdateAsync(1, request, CancellationToken.None);

        Assert.True(updated);
        var game = await _db.Games.AsNoTracking().SingleAsync(g => g.Id == 1);
        Assert.Equal(request.Title, game.Title);
        Assert.Equal(Platform.PlayStation5, game.Platform);
        Assert.Equal(request.ReleaseDate, game.ReleaseDate);
        Assert.Equal(request.Price, game.Price);
    }

    [Fact]
    public async Task Update_UnknownId_ReturnsFalse()
    {
        var updated = await _service.UpdateAsync(999, TestGames.ValidUpdate(), CancellationToken.None);

        Assert.False(updated);
    }
}
