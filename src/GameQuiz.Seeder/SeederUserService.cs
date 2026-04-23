using GameQuiz.Application.Interfaces;

namespace GameQuiz.Seeder;

internal class SeederUserService : ICurrentUserService
{
    public string? UserId => "seeder";
}
