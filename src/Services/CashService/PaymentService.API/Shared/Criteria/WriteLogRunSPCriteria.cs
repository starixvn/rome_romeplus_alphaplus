using PaymentService.API.Shared.Helpers;
using Fanex.Data;
using Fanex.Data.Repository;

namespace PaymentService.API.Payments.Criteria;

public class WriteLogRunSPCriteria(
    int userId,
    int custId,
    int actionId,
    string trackingValue,
    string trackingValue2) : NonQueryCommand
{
    public int UserId { get; } = userId;

    public int CustId { get; } = custId;

    public int ActionId { get; } = actionId;

    public string TrackingValue { get; } = trackingValue;

    public string TrackingValue2 { get; } = trackingValue2;

    [SpParam(Ignored = true)]
    public int? CustDBGroupId { get; set; }

    public override string GetSettingKey()
        => SPNameHelper.CustDBPrefix("WriteLog_UserLog_RunSP", CustDBGroupId);
    public override bool IsValid() => CustId > 0;
}