using NewtonCatalog.Api.Generated;

namespace NewtonCatalog.Api.Services;

public interface IGameService
{
    Task<List<Game>> GetAllAsync(CancellationToken ct);

    Task<Game?> GetByIdAsync(int id, CancellationToken ct);

    Task<bool> UpdateAsync(int id, UpdateGameRequest request, CancellationToken ct);
}
