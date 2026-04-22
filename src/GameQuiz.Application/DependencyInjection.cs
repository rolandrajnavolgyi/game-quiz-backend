using GameQuiz.Application.Interfaces;
using GameQuiz.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GameQuiz.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
       => services
           .AddServices();

    private static IServiceCollection AddServices(this IServiceCollection services)
        => services
            .AddScoped<IGameService, GameService>();
}
