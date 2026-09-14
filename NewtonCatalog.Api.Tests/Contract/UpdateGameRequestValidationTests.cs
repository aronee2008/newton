using System.ComponentModel.DataAnnotations;
using NewtonCatalog.Api.Generated;

namespace NewtonCatalog.Api.Tests.Contract;

public class UpdateGameRequestValidationTests
{
    [Fact]
    public void ValidRequest_HasNoErrors()
    {
        Assert.Empty(Validate(TestGames.ValidUpdate()));
    }

    [Fact]
    public void EmptyTitle_IsRejected()
    {
        var request = TestGames.ValidUpdate();
        request.Title = "";

        Assert.Contains(Validate(request), r => r.MemberNames.Contains("Title"));
    }

    [Fact]
    public void TitleOver100Characters_IsRejected()
    {
        var request = TestGames.ValidUpdate();
        request.Title = new string('x', 101);

        Assert.Contains(Validate(request), r => r.MemberNames.Contains("Title"));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void RatingOutsideZeroToHundred_IsRejected(int rating)
    {
        var request = TestGames.ValidUpdate();
        request.Rating = rating;

        Assert.Contains(Validate(request), r => r.MemberNames.Contains("Rating"));
    }

    [Fact]
    public void NegativePrice_IsRejected()
    {
        var request = TestGames.ValidUpdate();
        request.Price = -0.01m;

        Assert.Contains(Validate(request), r => r.MemberNames.Contains("Price"));
    }

    private static List<ValidationResult> Validate(UpdateGameRequest request)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(request, new ValidationContext(request), results, validateAllProperties: true);
        return results;
    }
}
