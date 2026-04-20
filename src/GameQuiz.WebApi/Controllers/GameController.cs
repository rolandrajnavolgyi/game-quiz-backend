using GameQuiz.Application.DTOs;
using GameQuiz.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameQuiz.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameController : ControllerBase
{
    private readonly IGameService _gameService;

    public GameController(IGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameDTO>>> GetAll()
    {
        var products = await _gameService.GetAllAsync();
        return Ok(products);
    }

}
