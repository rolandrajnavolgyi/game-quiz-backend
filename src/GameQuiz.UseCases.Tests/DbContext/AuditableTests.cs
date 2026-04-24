using FluentAssertions;
using GameQuiz.Domain.Entities;
using GameQuiz.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Time.Testing;

namespace GameQuiz.UseCases.Tests.DbContext;

public class AuditableTests
{
    [Fact]
    public async Task Should_SetAuditableProperties_WhenGameIsCreated()
    {
        // Arrange
        var timeProvider = new FakeTimeProvider();
        timeProvider.SetUtcNow(new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero));
        using var context = new TestDbContextFactory().CreateDbContext(timeProvider);
        var ct = TestContext.Current.CancellationToken;

        var repo = new GameRepository(context);
        var game = new Game { Name = "The Game" };
        
        context.Games.Add(game);
        await context.SaveChangesAsync(ct);

        // Act
        var savedGame = await context.Games.SingleAsync(ct);

        // Assert
        savedGame.Should().NotBeNull();
        savedGame.Name.Should().Be(game.Name);
        savedGame.CreatedAtUtc.Should().Be(timeProvider.GetUtcNow().UtcDateTime);
        savedGame.CreatedBy.Should().Be(TestUserService.TestUserId);
    }
}
