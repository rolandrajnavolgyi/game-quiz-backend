namespace GameQuiz.Domain.Interfaces;

public interface IAuditable
{
    DateTime CreatedAtUtc { get; }
    string? CreatedBy { get; }
}
