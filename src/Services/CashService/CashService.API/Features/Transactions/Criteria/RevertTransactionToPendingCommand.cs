namespace AlphaPlus.CashService.API.Features.Transactions.Criteria;

using Fanex.Data;
using Fanex.Data.Repository;

public class RevertTransactionToPendingCommand : NonQueryCommand
{
    public long TransId { get; set; }
    public string Remark { get; set; }
    public int UserId { get; set; }
    public string Ip { get; set; }

    [SpParam(Ignored = true)]
    public int SiteDbGroupId { get; set; }

    public int ErrCode { get; set; }
    public string ErrMsg { get; set; }

    public override string GetSettingKey() => $"SiteDb_{SiteDbGroupId}_Age_CashTransaction_RevertPending";

    public override bool IsValid() => TransId > 0;
}