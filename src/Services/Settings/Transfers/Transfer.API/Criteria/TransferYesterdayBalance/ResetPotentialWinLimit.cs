using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferYesterdayBalance;

public class ResetPotentialWinLimit : NonQueryCommand
{
    public ResetPotentialWinLimit(IEnumerable<int> custIds)
    {
        ListCustId = custIds != null ? string.Join(",", custIds) : string.Empty;
    }

    public string ListCustId { get; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override bool IsValid() => !string.IsNullOrEmpty(ListCustId);

    public override string GetSettingKey() => "SiteDB_CustWinLimitBet_Reset";
}