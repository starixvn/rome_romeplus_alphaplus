namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferYesterdayBalance;

public class DirectMemberTransferYesterdayBalanceCriteria(
    int memberId,
    string superOrMasterName,
    decimal amount,
    string language,
    string ip,
    int creator,
    int subAccId) : TransferYesterdayBalanceCriteria(language, ip, creator, subAccId)
{
    public int FromCustId { get; } = memberId;

    public string ToUsername { get; } = superOrMasterName;

    public decimal Amount { get; } = amount;

    public override string GetSettingKey() => "SiteDB_Age_Transfer_Ins_DirectMember";

    public override bool IsValid() => FromCustId > 0 && !string.IsNullOrWhiteSpace(ToUsername);
}