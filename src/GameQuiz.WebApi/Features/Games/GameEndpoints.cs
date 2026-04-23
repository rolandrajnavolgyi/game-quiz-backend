namespace GameQuiz.WebApi.Features.Games;

internal static class GameEndpoints
{
    public static IEndpointRouteBuilder MapGameEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/games")
                       .WithTags("Games");

        group.MapGet("/", GameOperations.GetAllGames)
             .WithName(nameof(GameOperations.GetAllGames))
             .WithSummary("Get all valid games from cache or database");

        return app;
    }
}
