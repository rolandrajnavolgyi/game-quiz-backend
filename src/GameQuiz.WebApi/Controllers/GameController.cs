using GameQuiz.Application.DTOs;
using GameQuiz.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;

namespace GameQuiz.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameController : ControllerBase
{
    private readonly IGameService _gameService;
    private readonly HybridCache _cache;

    public GameController(IGameService gameService, HybridCache cache)
    {
        _gameService = gameService;
        _cache = cache;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameDTO>>> GetAll(CancellationToken cancellationToken)
    {
        var cachedProducts = await _cache.GetOrCreateAsync("games", async token =>
        { 
            return await _gameService.GetAllAsync(token);
        }, cancellationToken: cancellationToken);

        return cachedProducts is null ? NotFound() : Ok(cachedProducts);
    }
}