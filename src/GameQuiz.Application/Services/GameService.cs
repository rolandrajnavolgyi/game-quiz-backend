using GameQuiz.Application.DTOs;
using GameQuiz.Application.Interfaces;
using GameQuiz.Domain.Interfaces;

namespace GameQuiz.Application.Services;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;

    public GameService(IGameRepository gameRepository)
    {
        _gameRepository = gameRepository;
    }

    public async Task<IEnumerable<GameDTO>> GetAllAsync(CancellationToken cancellationToken)
    {
        var games = await _gameRepository.GetAllAsync(cancellationToken);
        return games.Select(g => new GameDTO(g.Id, g.Name));
    }
}
