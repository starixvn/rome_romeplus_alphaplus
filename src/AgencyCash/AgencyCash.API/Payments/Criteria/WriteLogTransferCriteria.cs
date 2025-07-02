using AgencyCash.API.Shared.Helpers;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Criteria;

public class WriteLogTransferCriteria(
    int userId,
    int fromCustId,
    int toCustId,
    string ip,
    string doing) : NonQueryCommand
{
    public int UserId { get; } = userId;

    public int FromCustId { get; } = fromCustId;

    public int ToCustId { get; } = toCustId;

    [SpParam(Ignored = true)]
    public short? SiteDBGroupId { get; set; }

    public string IP { get; } = ip;

    public string Doing { get; } = doing;

    public override string GetSettingKey()
        => SiteDBGroupId.HasValue
        ? SPNameHelper.SiteDBPrefix("WriteLog_CashService_UserLog_Transfer", SiteDBGroupId.Value)
        : "DBLOG_WriteLog_CashService_UserLog_Transfer";

    public override bool IsValid()
    {
        if (FromCustId <= 0 || ToCustId <= 0)
        {
            return false;
        }

        if (UserId <= 0 || string.IsNullOrEmpty(IP) || string.IsNullOrEmpty(Doing))
        {
            return false;
        }

        return true;
    }
}