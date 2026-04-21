using GameQuiz.Application.DTOs;
using GameQuiz.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;

namespace GameQuiz.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameController : ControllerBase
{
    private readonly IGameService _gameService;
    private readonly ILogger<GameController> _logger;

    public GameController(IGameService gameService, ILogger<GameController> logger)
    {
        _gameService = gameService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameDTO>>> GetAll()
    {
        using (LogContext.PushProperty("User", "me"))
        {
            _logger.LogWarning("Method called at {Path}", Request.Path);
        }
        var products = await _gameService.GetAllAsync();
        return Ok(products);
    }

    [HttpGet]
    [Route("error")]
    public async Task<IActionResult> TriggerError()
    {
        throw new NotImplementedException();
    }

}
