namespace AlphaPlus.Settings.Transfer.API.Criteria.AutoTransfer;

using Core.Criteria;
using Core.Helpers;

public class CancelJobCriteria(
    int fromCustId,
    int toCustId,
    string viewStatus,
    string language,
    string ip,
    int siteDbGroupId)
    : SiteDbBaseCommand(siteDbGroupId)
{
    public int FromCustId { get; } = fromCustId;

    public int ToCustId { get; } = toCustId;

    public string ViewStatus { get; } = viewStatus;

    public int Creator => FromCustId;

    public string Language { get; set; } = language;

    public string IP { get; set; } = ip;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; } = string.Empty;

    public override string GetSettingKey() => SpNameHelpers.SiteDbPrefix("Age_ClearBalanceCust_ProcessCancel", SiteDbGroupId);

    public override bool IsValid() => FromCustId > 0 && ToCustId > 0;
}