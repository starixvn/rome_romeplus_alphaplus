namespace AlphaPlus.Reports.Features.Shared.Models;

public class Result(int code, string message)
{
    public int Code { get; } = code;

    public string Message { get; set; } = message;

    public bool IsOk { get; } = code == 0;
}