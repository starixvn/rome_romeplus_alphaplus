namespace AlphaPlus.CashService.Site.Features.Shared.Models;

public class ResultModel
{
    public int Code { get; set; } = 1;
    public string Message { get; set; } = string.Empty;
    public bool IsSuccessful => Code == 0;
    public Dictionary<string, string> Data { get; set; }

    public ResultModel()
    {
    }

    public ResultModel(int code, string message)
    {
        Code = code;
        Message = message;
    }

    public string GetDataValue(string key)
    {
        if (Data is IDictionary<string, string> dict && dict.TryGetValue(key, out var value))
        {
            return value ?? string.Empty;
        }
        return string.Empty;
    }
}