namespace AlphaPlus.CashService.API.Features.Transactions.Criteria;

using Fanex.Data.Repository;

public class RevertWithdrawalTransactionToPendingCommand : NonQueryCommand
{
    public long TransId { get; set; }
    public string Remark { get; set; }
    public int UserId { get; set; }
    public string Ip { get; set; }
    public int SiteDbGroupId { get; set; }

    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }

    public override string GetSettingKey() => $"SiteDb_{SiteDbGroupId}_Age_CashTransaction_RevertWithdrawal_Upd";

    public override bool IsValid() => TransId > 0;
}