using PaymentService.API.Shared.Helpers;
using Fanex.Data;
using Fanex.Data.Repository;

namespace PaymentService.API.Payments.Criteria;

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

    public string IP { get; } = ip;

    public string Doing { get; } = doing;

    [SpParam(Ignored = true)]
    public int SiteDBGroupId { get; set; }

    public override string GetSettingKey()
        => SPNameHelper.SiteDBPrefix("WriteLog_UserLog_Transfer", SiteDBGroupId);

    public override bool IsValid() => FromCustId > 0 && ToCustId > 0;
}