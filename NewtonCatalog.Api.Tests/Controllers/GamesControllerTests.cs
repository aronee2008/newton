using Microsoft.AspNetCore.Mvc;
using NewtonCatalog.Api.Controllers;
using NewtonCatalog.Api.Generated;
using NewtonCatalog.Api.Services;
using NSubstitute;

namespace NewtonCatalog.Api.Tests.Controllers;

public class GamesControllerTests
{
    private readonly IVideoGameService _service = Substitute.For<IVideoGameService>();
    private readonly GamesController _controller;

    public GamesControllerTests()
    {
        _controller = new GamesController(_service);
    }

    [Fact]
    public async Task GetAll_ReturnsAllGames()
    {
        _service.GetAllAsync(Arg.Any<CancellationToken>()).Returns([TestGames.Hades(), TestGames.Celeste()]);

        var result = await _controller.GetAll(CancellationToken.None);

        Assert.Equal(2, result.Value!.Count);
    }

    [Fact]
    public async Task GetById_ExistingGame_ReturnsGame()
    {
        _service.GetByIdAsync(1, Arg.Any<CancellationToken>()).Returns(TestGames.Hades());

        var result = await _controller.GetById(1, CancellationToken.None);

        Assert.Equal("Hades", result.Value!.Title);
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
