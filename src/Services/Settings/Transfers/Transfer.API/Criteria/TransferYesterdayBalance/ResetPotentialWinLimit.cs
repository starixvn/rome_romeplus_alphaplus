using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferYesterdayBalance;

public class ResetPotentialWinLimit(IEnumerable<int> custIds) : NonQueryCommand
{
    public string CustIds { get; } = string.Join(",", custIds);

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }
    
    public override bool IsValid() => !string.IsNullOrEmpty(CustIds);

    public override string GetSettingKey() => "SiteDB_CustWinLimitBet_Reset";
}