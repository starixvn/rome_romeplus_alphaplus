namespace AlphaPlus.CashService.API.Features.Shared.Models;

public class ResultModel(int code, string message)
{
    public ResultModel() : this(1, "Data Error")
    {
    }

    public int Code { get; set; } = code;
    public string Message { get; set; } = message;
    public bool IsSuccessful => Code == 0;
    public string ErrExtraInfo { get; set; }
    public Dictionary<string, string> Data { get; set; }
}