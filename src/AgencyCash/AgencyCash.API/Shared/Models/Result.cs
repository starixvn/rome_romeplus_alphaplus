namespace AgencyCash.API.Shared.Models;

public record Result(int Code, string Message)
{
    public bool IsSuccess { get; } = Code == 0;
}
