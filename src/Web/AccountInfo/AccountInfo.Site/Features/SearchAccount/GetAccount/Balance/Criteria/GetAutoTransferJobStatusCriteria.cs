namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.Balance.Criteria;

using Core.Criteria;
using Core.Helpers;

public class GetAutoTransferJobStatusCriteria : SiteDbBaseCommand
{
    public int FromCustId { get; }

    public int ToCustId { get; }

    public bool IsAllowAutoTransfer => true;

    public bool IsAutoJobRunning { get; set; }

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; } = string.Empty;

    public override string GetSettingKey() => SpNameHelpers.SiteDbPrefix("Age_ClearBalanceCust_Job_StatusCheck", SiteDbGroupId);

    public override bool IsValid() => true;
}