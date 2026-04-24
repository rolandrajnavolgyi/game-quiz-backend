using GameQuiz.Application.DTOs;
using GameQuiz.Application.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Caching.Hybrid;

namespace GameQuiz.WebApi.Features.Games;

internal static class GameOperations
{
    public static async Task<Ok<IEnumerable<GameDTO>>> GetAllGames(IGameService gameService, 
        HybridCache cache, CancellationToken cancellationToken)
    {
        var cachedGames = await cache.GetOrCreateAsync("games", async token =>
        {
            return await gameService.GetAllAsync(token);
        }, cancellationToken: cancellationToken);

        return TypedResults.Ok(cachedGames);
    }
}