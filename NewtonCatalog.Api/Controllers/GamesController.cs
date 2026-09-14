using Microsoft.AspNetCore.Mvc;
using NewtonCatalog.Api.Generated;
using NewtonCatalog.Api.Services;

namespace NewtonCatalog.Api.Controllers;

[ApiController]
public class GamesController(IVideoGameService games) : GamesControllerBase
{
    public override async Task<ActionResult<ICollection<Game>>> GetAll(CancellationToken cancellationToken) =>
        await games.GetAllAsync(cancellationToken);

    public override async Task<ActionResult<Game>> GetById(int id, CancellationToken cancellationToken)
    {
        var game = await games.GetByIdAsync(id, cancellationToken);
        return game is null ? NotFound() : game;
    }

    public override async Task<IActionResult> Update(UpdateGameRequest body, int id, CancellationToken cancellationToken)
    {
        var updated = await games.UpdateAsync(id, body, cancellationToken);
        return updated ? NoContent() : NotFound();
    }
}
