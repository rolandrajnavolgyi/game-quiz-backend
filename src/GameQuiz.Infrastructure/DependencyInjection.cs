using GameQuiz.Domain.Interfaces;
using GameQuiz.Infrastructure.Data;
using GameQuiz.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameQuiz.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
       => services
           .AddRepositories()
           .AddSqlServer(configuration);

    private static IServiceCollection AddRepositories(this IServiceCollection services)
        => services
            .AddScoped<IGameRepository, GameRepository>();

    private static IServiceCollection AddSqlServer(this IServiceCollection services, IConfiguration configuration)
        => services
            .AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("GameQuizDB"), sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                }));
}