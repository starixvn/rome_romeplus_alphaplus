namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferYesterdayBalance;

using Core.Criteria;
using Core.Helpers;

public class ResetPotentialWinLimit(IEnumerable<int> custIds) : SiteDbBaseCommand
{
    public string ListCustId { get; } = custIds != null ? string.Join(",", custIds) : string.Empty;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override bool IsValid() => !string.IsNullOrEmpty(ListCustId);

    public override string GetSettingKey() => SpNameHelpers.SiteDbPrefix("Acc_CustWinLimitBet_Reset", SiteDbGroupId);
}