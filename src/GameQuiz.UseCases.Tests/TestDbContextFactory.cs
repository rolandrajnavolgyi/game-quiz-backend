using GameQuiz.Infrastructure.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Time.Testing;

namespace GameQuiz.UseCases.Tests;

internal class TestDbContextFactory : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public TestDbContextFactory()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = CreateDbContext();
        context.Database.EnsureCreated();
    }

    public ApplicationDbContext CreateDbContext()
        => new ApplicationDbContext(_options, new TestUserService(), TimeProvider.System);

    public ApplicationDbContext CreateDbContext(FakeTimeProvider timeProvider)
        => new ApplicationDbContext(_options, new TestUserService(), timeProvider);

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }
}
