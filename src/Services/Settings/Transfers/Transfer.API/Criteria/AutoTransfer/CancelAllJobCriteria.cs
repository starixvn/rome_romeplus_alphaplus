namespace AlphaPlus.Settings.Transfer.API.Criteria.AutoTransfer;

using Core.Criteria;
using Core.Helpers;

public class CancelAllJobCriteria(int fromCustId, string language, string ip) : SiteDbBaseCommand
{
    public int FromCustId { get; } = fromCustId;

    public string Language { get; set; } = language;

    public string IP { get; set; } = ip;

    public int Creator => FromCustId;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; } = string.Empty;

    public override string GetSettingKey() => SpNameHelpers.SiteDbPrefix("Age_ClearBalanceCust_ProcessCancel_All", SiteDbGroupId);

    public override bool IsValid() => FromCustId > 0;
}