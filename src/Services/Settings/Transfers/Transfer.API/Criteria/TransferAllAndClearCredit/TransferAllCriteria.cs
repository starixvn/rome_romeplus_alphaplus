namespace AlphaPlus.Settings.Transfer.API.Criteria.TransferAllAndClearCredit;

using Core.Criteria;
using Core.Helpers;
using Fanex.Data;

public class TransferAllCriteria(
    int custId,
    int userId,
    int subAccId,
    string username,
    string ip,
    string language,
    bool isAllDayBalanceTransfer,
    int siteDbGroupId)
    : SiteDbBaseCriteria(siteDbGroupId)
{
    public int CustId { get; set; } = custId;

    public int UserId { get; set; } = userId;

    public int SubAccId { get; set; } = subAccId;

    [SpParam(SqlParamName = "DownlineUsername")]
    public string Username { get; set; } = username;

    [SpParam(SqlParamName = "IsAllDayTransfer")]
    public bool IsAllDayBalanceTransfer { get; set; } = isAllDayBalanceTransfer;

    public string IP { get; set; } = ip;

    public string Language { get; set; } = language;

    public int ErrCode { get; set; }

    public string ErrMsg { get; set; }

    public override string GetSettingKey()
        => SpNameHelpers.SiteDbPrefix("Age_Transfer_All_Ins", SiteDbGroupId);

    public override bool IsValid()
        => !string.IsNullOrWhiteSpace(Username);
}