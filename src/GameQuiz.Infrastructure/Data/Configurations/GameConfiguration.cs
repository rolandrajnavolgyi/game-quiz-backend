using GameQuiz.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GameQuiz.Infrastructure.Data.Configurations;

internal class GameConfiguration : EntityConfiguration<Game>
{
    public override void Configure(EntityTypeBuilder<Game> builder)
    {
        base.Configure(builder);

        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}
