using AgencyCash.API.Shared.Helpers;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AgencyCash.API.Shared.Criteria;

public class AddRequestLoggingCriteria(
    int userId,
    int custId,
    string funcName,
    string requestUrl,
    string requestBody,
    string from = "Member Site",
    string to = "Agency Cash API") : NonQueryCommand
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
        ? SPNameHelper.CustDBPrefix("WriteLog_CashService_UserLog_API", CustDBGroupId.Value)
        : "DBLOG_WriteLog_CashService_UserLog_API";

    public override bool IsValid() => true;
}