using GameQuiz.Domain.Entities;
using GameQuiz.Domain.Interfaces;
using GameQuiz.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GameQuiz.Infrastructure.Repositories;

public class GameRepository : IGameRepository
{
    private readonly ApplicationDbContext _context;

    public GameRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Game>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Games.ToListAsync(cancellationToken);
    }
}
