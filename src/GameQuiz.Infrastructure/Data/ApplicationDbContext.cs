using GameQuiz.Application.Interfaces;
using GameQuiz.Domain.Entities;
using GameQuiz.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameQuiz.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    private readonly ICurrentUserService _currentUserService;
    private readonly TimeProvider _timeProvider;

    public DbSet<Game> Games { get; set; }

    public ApplicationDbContext(ICurrentUserService currentUserService, 
        TimeProvider timeProvider)
    {
        _currentUserService = currentUserService;
        _timeProvider = timeProvider;
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService currentUserService,
        TimeProvider timeProvider)
        : base(options)
    {
        _currentUserService = currentUserService;
        _timeProvider = timeProvider;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(IAuditable).IsAssignableFrom(entity.ClrType))
            {
                var builder = modelBuilder.Entity(entity.ClrType);

                builder.Property(nameof(Entity.CreatedAtUtc))
                    .ValueGeneratedNever();
                builder.Property(nameof(Entity.CreatedBy))
                    .ValueGeneratedNever();
            }
        }

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override int SaveChanges()
    {
        ApplyAudit();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyAudit();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAudit();
        return await base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ApplyAudit();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void ApplyAudit()
    {
        var utcNow = _timeProvider.GetUtcNow().UtcDateTime;
        var userId = _currentUserService.UserId ?? "system";

        foreach (var entry in ChangeTracker.Entries<IAuditable>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(x => x.CreatedAtUtc).CurrentValue = utcNow;
                entry.Property(x => x.CreatedBy).CurrentValue = userId;
            }
        }
    }
}
