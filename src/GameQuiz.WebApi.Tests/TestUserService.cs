using GameQuiz.Application.Interfaces;

namespace GameQuiz.WebApi.Tests;

internal class TestUserService : ICurrentUserService
{
    public const string TestUserId = "test-user";

    public string? UserId { get; set; } = TestUserId;
}
