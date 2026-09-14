using Microsoft.AspNetCore.Mvc;
using NewtonCatalog.Api.Controllers;
using NewtonCatalog.Api.Generated;
using NewtonCatalog.Api.Services;
using NSubstitute;

namespace NewtonCatalog.Api.Tests.Controllers;

public class GamesControllerTests
{
    private readonly IGameService _service = Substitute.For<IGameService>();
    private readonly GamesController _controller;

    public GamesControllerTests()
    {
        _controller = new GamesController(_service);
    }

    [Fact]
    public async Task GetById_UnknownGame_ReturnsNotFound()
    {
        _service.GetByIdAsync(999, Arg.Any<CancellationToken>()).Returns((Game?)null);

        var result = await _controller.GetById(999, CancellationToken.None);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Update_ExistingGame_ReturnsNoContent()
    {
        _service.UpdateAsync(1, Arg.Any<UpdateGameRequest>(), Arg.Any<CancellationToken>()).Returns(true);

        var result = await _controller.Update(TestGames.ValidUpdate(), 1, CancellationToken.None);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_UnknownGame_ReturnsNotFound()
    {
        _service.UpdateAsync(999, Arg.Any<UpdateGameRequest>(), Arg.Any<CancellationToken>()).Returns(false);

        var result = await _controller.Update(TestGames.ValidUpdate(), 999, CancellationToken.None);

        Assert.IsType<NotFoundResult>(result);
    }
}
