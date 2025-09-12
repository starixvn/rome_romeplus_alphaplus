namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Balance.Criteria;

using Core.Criteria;
using Core.Helpers;

public class GetAutoTransferJobStatusCriteria(int siteDbGroupId) : SiteDbBaseCommand(siteDbGroupId)
{
    public int FromCustId { get; } = 0;

    public int ToCustId { get; } = 0;

    public bool IsAllowAutoTransfer { get; } = true;

    public bool IsAutoJobRunning { get; set; }

    public int ErrCode { get; set; } = 0;

    public string ErrMsg { get; set; } = string.Empty;

    public override string GetSettingKey() => SpNameHelpers.SiteDbPrefix("Age_ClearBalanceCust_Job_StatusCheck", SiteDbGroupId);

    public override bool IsValid() => true;
}