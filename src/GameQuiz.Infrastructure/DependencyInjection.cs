using GameQuiz.Application.Interfaces;
using GameQuiz.Domain.Interfaces;
using GameQuiz.Infrastructure.Data;
using GameQuiz.Infrastructure.Repositories;
using GameQuiz.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameQuiz.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
       => services
           .AddServices()
           .AddRepositories()
           .AddSqlServer(configuration);

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
        => services
            .AddScoped<IGameRepository, GameRepository>();

    private static IServiceCollection AddSqlServer(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddSingleton(TimeProvider.System)
            .AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("GameQuizDB"), sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                }));
}