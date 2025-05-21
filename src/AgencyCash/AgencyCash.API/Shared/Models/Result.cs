namespace AgencyCash.API.Shared.Models;

public record Result(int Code, string Message)
{
    public static Result SystemError
        => new Result(500, "Server Error.");

    public bool IsSuccess { get; } = Code == 0;
}


