using NewtonCatalog.Api.Generated;

namespace NewtonCatalog.Api.Repositories;

public interface IVideoGameRepository
{
    Task<List<Game>> GetAllAsync(CancellationToken ct);

    Task<Game?> GetByIdAsync(int id, CancellationToken ct);

    Task UpdateAsync(Game game, UpdateGameRequest changes, CancellationToken ct);
}
