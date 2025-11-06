namespace AlphaPlus.Settings.Transfer.API.Criteria.AutoTransfer;

using Core.Criteria;
using Core.Helpers;

public class GetTodayTransfersCriteria(int siteDbGroupId) : SiteDbBaseCommand(siteDbGroupId)
{
    public int FromCustId { get; set; }

    public string Language { get; set; }

    public string Searchname { get; set; }

    public string ViewStatus { get; set; }

    public byte LevelType { get; set; }

    public int SortBy { get; set; }

    public int SortType { get; set; }

    public int PageSize { get; set; }

    public int PageIndex { get; set; }

    public int TotalRow { get; set; }

    public int TotalCancelAll { get; set; }
    public int TotalPending { get; set; }
    public int TotalProcessing { get; set; }
    public int TotalFailed { get; set; }

    public override bool IsValid() => !string.IsNullOrWhiteSpace(ViewStatus);

    public override string GetSettingKey() => SpNameHelpers.SiteDbPrefix("Age_ClearBalanceCust_ProcessSel", SiteDbGroupId);
}