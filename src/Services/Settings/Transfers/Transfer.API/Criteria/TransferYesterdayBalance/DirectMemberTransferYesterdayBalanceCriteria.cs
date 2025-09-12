namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferYesterdayBalance;

using Core.Helpers;

public class DirectMemberTransferYesterdayBalanceCriteria(
    int memberId,
    string superOrMasterName,
    decimal amount,
    string language,
    string ip,
    int creator,
    int subAccId,
    int siteDbGroupId) : TransferYesterdayBalanceCriteria(language, ip, creator, subAccId, siteDbGroupId)
{
    public int FromCustId { get; } = memberId;

    public string ToUsername { get; } = superOrMasterName;

    public decimal Amount { get; } = amount;

    public override string GetSettingKey() => SpNameHelpers.SiteDbPrefix("Age_Transfer_Ins_DirectMember", SiteDbGroupId);

    public override bool IsValid() => FromCustId > 0 && !string.IsNullOrWhiteSpace(ToUsername);
}