using AgencyCash.API.Shared.Helpers;
using Fanex.Data.Repository;

namespace AgencyCash.API.Payments.Criteria;

public class ManualWithdrawalTransferCriteria : NonQueryCommand
{
    public ManualWithdrawalTransferCriteria(
        int custId,
        int uplineId,
        decimal exchangeRate,
        string ip,
        decimal amount,
        short siteDBGroupId)
    {
        CustId = custId;
        UplineId = uplineId;
        ExchangeRate = exchangeRate;
        IP = ip;
        Amount = amount;
        SiteDBGroupId = siteDBGroupId;
    }

    public int CustId { get; }
    public int UplineId { get; }
    public decimal ExchangeRate { get; }
    public string IP { get; }
    public decimal Amount { get; }
    public short SiteDBGroupId { get; }

    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }
    public long TransferRefNo { get; set; }

    public override string GetSettingKey()
        => SPNameHelper.SiteDBPrefix("Age_CashWithdrawal_ManualAdd_InsTransfer", SiteDBGroupId);

    public override bool IsValid() => CustId > 0 && UplineId > 0;
} 