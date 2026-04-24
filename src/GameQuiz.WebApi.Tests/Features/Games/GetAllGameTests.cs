using FluentAssertions;
using GameQuiz.Application.DTOs;
using GameQuiz.Domain.Entities;
using System.Net;
using System.Net.Http.Json;

namespace GameQuiz.WebApi.Tests.Features.Games;

public class GetAllGameTests : TestBase
{
    public GetAllGameTests(TestApplicationFactory factory) : base(factory) { }

    [Fact]
    public async Task Should_ReturnGameDTOList_WhenGamesExist()
    {
        // Arrange
        var ct = TestContext.Current.CancellationToken;
        var gameOne = new Game { Name = "One" };
        var gameTwo = new Game { Name = "Two" };
        
        await SeedAsync(db =>
        {
            db.Games.AddRange([gameOne, gameTwo]);
            return Task.CompletedTask;
        });

        // Act
        var response = await client.GetAsync("/api/games", ct); 

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var result = await response.Content.ReadFromJsonAsync<List<GameDTO>>(ct);
        result.Should().SatisfyRespectively(
            first => first.Name.Should().Be(gameOne.Name),
            second => second.Name.Should().Be(gameTwo.Name)
        );
    }
}
