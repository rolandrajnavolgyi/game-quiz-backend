using GameQuiz.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace GameQuiz.WebApi.Tests;

public abstract class TestBase : IClassFixture<TestApplicationFactory>, IAsyncLifetime
{
    protected readonly HttpClient client;
    protected readonly IServiceScopeFactory scopeFactory;

    protected TestBase(TestApplicationFactory factory)
    {
        client = factory.CreateClient();
        scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
    }

    protected async Task SeedAsync(Func<ApplicationDbContext, Task> seed)
    {
        using var scope = scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await seed(db);
        await db.SaveChangesAsync();
    }

    protected async Task<T> ExecuteDbAsync<T>(Func<ApplicationDbContext, Task<T>> query)
    {
        using var scope = scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        return await query(db);
    }

    public async ValueTask InitializeAsync()
    {
        using var scope = scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await db.Database.EnsureDeletedAsync();
        await db.Database.EnsureCreatedAsync();
    }

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
