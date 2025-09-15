namespace AlphaPlus.Settings.Transfer.API.Criteria.AutoTransfer;

using Core.Criteria;
using Core.Helpers;

public class GetJobStatusCriteria(int? fromCustId, int? toCustId, bool isAllowAutoTransfer, int siteDbGroupId) : SiteDbBaseCommand(siteDbGroupId)
{
    public int? FromCustId { get; } = fromCustId;

    public int? ToCustId { get; } = toCustId;

    public bool IsAllowAutoTransfer { get; } = isAllowAutoTransfer;

    public bool IsAutoJobRunning { get; set; }

    public int ErrCode { get; set; } = 0;

    public string ErrMsg { get; set; } = string.Empty;

    public override string GetSettingKey() => SpNameHelpers.SiteDbPrefix("Age_ClearBalanceCust_Job_StatusCheck", SiteDbGroupId);

    public override bool IsValid() => true;
}