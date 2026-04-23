using GameQuiz.Domain.Interfaces;

namespace GameQuiz.Domain.Entities;

public abstract class Entity: IAuditable
{
    public int Id { get; protected set; }
    public DateTime CreatedAtUtc { get; protected set; }
    public string? CreatedBy { get; protected set; }
}
