using AgencyCash.API.Shared.Helpers;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Criteria;

public class WriteLogRunSPCriteria(
    int userId,
    int custId,
    int actionId,
    string trackingValueMask,
    string trackingValueMask2,
    string trackingValue,
    string trackingValue2) : NonQueryCommand
{
    public int UserId { get; } = userId;

    public int CustId { get; } = custId;

    public int ActionId { get; } = actionId;

    public string TrackingValueMask { get; } = trackingValueMask;

    public string TrackingValueMask2 { get; } = trackingValueMask2;

    public string TrackingValue { get; } = trackingValue;

    public string TrackingValue2 { get; } = trackingValue2;

    [SpParam(Ignored = true)]
    public short? CustDBGroupId { get; set; }

    public override string GetSettingKey()
        => CustDBGroupId.HasValue
        ? SPNameHelper.CustDBPrefix("WriteLog_CashService_UserLog_RunSP", CustDBGroupId.Value)
        : "DBLOG_WriteLog_CashService_UserLog_RunSP";
    public override bool IsValid() => CustId > 0;
}