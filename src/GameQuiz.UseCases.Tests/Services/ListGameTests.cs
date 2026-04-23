using FluentAssertions;
using GameQuiz.Application.Services;
using GameQuiz.Domain.Entities;
using GameQuiz.Infrastructure.Repositories;

namespace GameQuiz.UseCases.Tests.Services;

public class ListGameTests
{
    [Fact]
    public async Task Should_ReturnEmptyList_WhenNoGamesExist()
    {
        // Arrange
        using var context = new TestDbContextFactory().CreateDbContext();
        using var cts = new CancellationTokenSource();

        var repo = new GameRepository(context);
        var service = new GameService(repo);

        // Act
        var result = await service.GetAllAsync(cts.Token);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_ReturnAllGames_WhenGamesExist()
    {
        // Arrange
        using var context = new TestDbContextFactory().CreateDbContext();
        using var cts = new CancellationTokenSource();
        
        var gameOne = new Game { Name = "One" };
        var gameTwo = new Game { Name = "Two" };

        context.Games.AddRange([gameOne, gameTwo]);
        await context.SaveChangesAsync();

        var repo = new GameRepository(context);
        var service = new GameService(repo);

        // Act
        var result = await service.GetAllAsync(cts.Token);

        // Assert
        result.Should().SatisfyRespectively(
            first => first.Name.Should().Be(gameOne.Name),
            second => second.Name.Should().Be(gameTwo.Name)
        );
    }

    [Fact]
    public async Task Should_ThrowException_WhenCanceled()
    {
        // Arrange
        using var context = new TestDbContextFactory().CreateDbContext();
        
        var repo = new GameRepository(context);
        var service = new GameService(repo);

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act & Assert
        await Assert.ThrowsAsync<OperationCanceledException>(() => service.GetAllAsync(cts.Token));
    }
}
