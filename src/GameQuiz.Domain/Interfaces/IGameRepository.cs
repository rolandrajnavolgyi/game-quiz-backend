using GameQuiz.Domain.Entities;

namespace GameQuiz.Domain.Interfaces;

public interface IGameRepository
{
    Task<IEnumerable<Game>> GetAllAsync(CancellationToken cancellationToken);
}
