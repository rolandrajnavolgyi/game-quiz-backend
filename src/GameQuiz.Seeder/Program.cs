using GameQuiz.Domain.Entities;
using GameQuiz.Infrastructure.Data;
using GameQuiz.Seeder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .AddSimpleConsole(options =>
        {
            options.TimestampFormat = "HH:mm:ss ";
        })
        .SetMinimumLevel(LogLevel.Debug);
});

var logger = loggerFactory.CreateLogger("Seeder");

try
{
    logger.LogInformation("Starting database seeding...");

    var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        .UseSqlServer(Environment.GetEnvironmentVariable("ConnectionStrings__GameQuizDB"), sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        })
        .Options;

    using var context = new ApplicationDbContext(options, new SeederUserService(), TimeProvider.System);

    if (!await context.Games.AnyAsync())
    {
        logger.LogInformation("No games found. Seeding data...");

        var json = await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory, "game_awards_nominees.json"));
        var gameNames = JsonSerializer.Deserialize<string[]>(json);
        var games = gameNames?
            .Distinct()
            .OrderBy(name => name)
            .Select(name => new Game { Name = name })
            .ToList();

        context.Games.AddRange(games!);
        await context.SaveChangesAsync();

        logger.LogInformation("Inserted {Count} games", games!.Count);
    }
    else
    {
        logger.LogInformation("Database already seeded. Skipping.");
    }

    logger.LogInformation("Seeding completed.");
}
catch (Exception ex)
{
    logger.LogError(ex, "Seeding failed!");
    Environment.Exit(1);
}