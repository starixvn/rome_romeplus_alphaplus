namespace AlphaPlus.CashService.API.Features.Shared.Criteria;

using Fanex.Data;
using Fanex.Data.Repository;

public class AddRequestLogCommand(int userId,
    int custId,
    string funcName,
    string requestUrl,
    string requestBody,
    string from = "Cash Service API",
    string to = "Payment Gateway API") : NonQueryCommand
{
    public int UserId { get; } = userId;
    public int CustId { get; } = custId;
    public string From { get; } = from;
    public string To { get; } = to;
    public string FuncName { get; } = funcName;
    public string RequestUrl { get; } = requestUrl;
    public string RequestBody { get; } = requestBody;
    public int StatusCode { get; set; }
    public string Exception { get; set; }
    public int Duration { get; set; }
    public string ResponseBody { get; set; }

    [SpParam(Ignored = true)]
    public short? CustDBGroupId { get; set; }

    public override string GetSettingKey()
        => CustDBGroupId.HasValue
            ? $"CustDb_{CustDBGroupId}_WriteLog_CashService_UserLog_API"
            : "DBLOG_WriteLog_CashService_UserLog_API";

    public override bool IsValid() => true;
}