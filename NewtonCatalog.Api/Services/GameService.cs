using NewtonCatalog.Api.Generated;
using NewtonCatalog.Api.Repositories;

namespace NewtonCatalog.Api.Services;

public class GameService(IGameRepository repository) : IGameService
{
    public Task<List<Game>> GetAllAsync(CancellationToken ct) =>
        repository.GetAllAsync(ct);

    public Task<Game?> GetByIdAsync(int id, CancellationToken ct) =>
        repository.GetByIdAsync(id, ct);

    public async Task<bool> UpdateAsync(int id, UpdateGameRequest request, CancellationToken ct)
    {
        var game = await repository.GetByIdAsync(id, ct);
        if (game is null)
        {
            return false;
        }

        await repository.UpdateAsync(game, request, ct);
        return true;
    }
}
