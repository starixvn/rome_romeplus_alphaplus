using Fanex.Data.Repository;

namespace AlphaPlus.Settings.Transfer.API.Criteria.CashBalanceTransfer;

public class AddCashBalanceTransferCriteria(
    int fromCustId,
    int toCustId,
    decimal amount,
    int creator,
    int subAccId,
    string ip,
    string languageKey,
    bool isDirectMember) : NonQueryCommand
{
    public int FromCustId { get; } = fromCustId;

    public int ToCustId { get; } = toCustId;

    public decimal Amount { get; } = amount;

    public int Creator { get; } = creator;

    public int SubAccId { get; } = subAccId;

    public string IP { get; set; } = ip;

    public string Language { get; set; } = languageKey;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; } = string.Empty;

    public override string GetSettingKey()
        => isDirectMember ? "SiteDB_Age_Transfer_CashBalance_DirectMember_Ins" : "SiteDB_Age_Transfer_CashBalance_Ins";

    public override bool IsValid()
        => FromCustId > 0 && ToCustId > 0 && Creator > 0 && Amount != 0;
}
