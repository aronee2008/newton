using Microsoft.EntityFrameworkCore;
using NewtonCatalog.Api.Data;
using NewtonCatalog.Api.Generated;

namespace NewtonCatalog.Api.Repositories;

public class GameRepository(CatalogDbContext db) : IGameRepository
{
    public Task<List<Game>> GetAllAsync(CancellationToken ct) =>
        db.Games.AsNoTracking().OrderBy(g => g.Title).ToListAsync(ct);

    public Task<Game?> GetByIdAsync(int id, CancellationToken ct) =>
        db.Games.FirstOrDefaultAsync(g => g.Id == id, ct);

    public Task UpdateAsync(Game game, UpdateGameRequest changes, CancellationToken ct)
    {
        db.Entry(game).CurrentValues.SetValues(changes);
        return db.SaveChangesAsync(ct);
    }
}
