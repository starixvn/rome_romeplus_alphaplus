namespace AlphaPlus.CashService.Site.Features.Shared.Models;

public class ResultModel
{
    public int Code { get; set; } = 1;
    public string Message { get; set; } = string.Empty;
    public bool IsSuccessful => Code == 0;

    public ResultModel()
    {
    }
    
    public ResultModel(int code, string message)
    {
        Code = code;
        Message = message;
    }
}