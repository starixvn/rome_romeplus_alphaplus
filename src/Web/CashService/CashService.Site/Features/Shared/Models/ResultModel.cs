namespace AlphaPlus.CashService.Site.Features.Shared.Models;

public class ResultModel
{
    public int Code { get; set; } = 1;
    public string Message { get; set; } = string.Empty;
    public bool IsSuccessful => Code == 0;
    public object? Data { get; set; }

    public ResultModel()
    {
    }

    public ResultModel(int code, string message)
    {
        Code = code;
        Message = message;
    }

    public ResultModel(int code, string message, object? data)
    {
        Code = code;
        Message = message;
        Data = data;
    }
}

public static class ResultModelExtensions
{
    public static string GetDataValue(this ResultModel result, string key)
    {
        if (result.Data is IDictionary<string, object> dict)
        {
            if (dict.TryGetValue(key, out var value))
            {
                return value?.ToString() ?? string.Empty;
            }
        }
        return string.Empty;
    }
}