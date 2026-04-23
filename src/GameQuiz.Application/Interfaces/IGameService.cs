using GameQuiz.Application.DTOs;

namespace GameQuiz.Application.Interfaces;

public interface IGameService
{
    Task<IEnumerable<GameDTO>> GetAllAsync(CancellationToken cancellationToken);
}
