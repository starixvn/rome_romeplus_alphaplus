namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferAllAndClearCredit;

using Core.Criteria;
using Core.Helpers;
using Fanex.Data;

public class GetCalculateResultCriteria(
    int custId,
    string username,
    string language,
    bool isAllDayBalanceTransfer,
    string downlineNetPosition,
    int downlineCustId,
    int siteDbGroupId)
    : SiteDbBaseCriteria(siteDbGroupId)
{
    public string DownlineNetPosition { get; set; } = downlineNetPosition;

    public int DownlineCustId { get; set; } = downlineCustId;

    public int CustId { get; set; } = custId;

    [SpParam(SqlParamName = "DownlineUsername")]
    public string Username { get; set; } = username;

    public string Language { get; set; } = language;

    [SpParam(SqlParamName = "IsAllDayTransfer")]
    public bool IsAllDayBalanceTransfer { get; set; } = isAllDayBalanceTransfer;

    public override string GetSettingKey() => SpNameHelpers.SiteDbPrefix("Age_Transfer_Credit_All_Sel", SiteDbGroupId);

    public override bool IsValid()
        => !string.IsNullOrWhiteSpace(Username) && DownlineCustId > 0;
}