namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferYesterdayBalance;

using Core.Helpers;

public class DirectMemberTransferMultipleYesterdayBalanceCriteria(
    int superOrMasterId,
    string toCustIds,
    int directMember,
    string status,
    byte autoTransfer,
    string language,
    string ip,
    int subAccId) : TransferYesterdayBalanceCriteria(language, ip, superOrMasterId, subAccId)
{
    public int FromCustId { get; } = superOrMasterId;

    public string ToCustIds { get; } = toCustIds;

    public int DirectMember { get; } = directMember;

    public string CustStatus { get; } = status;

    public byte AutoTransfer { get; } = autoTransfer;

    public override string GetSettingKey() => SpNameHelpers.SiteDbPrefix("Age_Transfer_InsClearBalance_DirectMember", SiteDbGroupId);

    public override bool IsValid() => FromCustId > 0 && !string.IsNullOrWhiteSpace(ToCustIds);
}