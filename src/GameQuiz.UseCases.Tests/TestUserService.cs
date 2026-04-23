using GameQuiz.Application.Interfaces;

namespace GameQuiz.UseCases.Tests;

internal class TestUserService : ICurrentUserService
{
    public const string TestUserId = "test-user";

    public string? UserId { get; set; } = TestUserId;
}
