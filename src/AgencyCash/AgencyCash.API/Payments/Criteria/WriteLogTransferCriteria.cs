using AgencyCash.API.Shared.Helpers;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Criteria;

public class WriteLogTransferCriteria : NonQueryCommand
{
    public WriteLogTransferCriteria(
        int userId,
        int fromCustId,
        int toCustId,
        string ip,
        string doing)
    {
        UserId = userId;
        FromCustId = fromCustId;
        ToCustId = toCustId;
        IP = ip;
        Doing = doing;
    }

    public int UserId { get; }

    public int FromCustId { get; }

    public int ToCustId { get; }

    [SpParam(Ignored = true)]
    public short? SiteDBGroupId { get; set; }

    public string IP { get; }

    public string Doing { get; }

    public override string GetSettingKey()
        => SiteDBGroupId.HasValue
        ? SPNameHelper.SiteDBPrefix("WriteLog_CashService_UserLog_Transfer", SiteDBGroupId.Value)
        : "DBLOG_WriteLog_CashService_UserLog_Transfer";

    public override bool IsValid() => FromCustId > 0 && ToCustId > 0 && UserId > 0 && !string.IsNullOrEmpty(IP) && !string.IsNullOrEmpty(Doing);
}