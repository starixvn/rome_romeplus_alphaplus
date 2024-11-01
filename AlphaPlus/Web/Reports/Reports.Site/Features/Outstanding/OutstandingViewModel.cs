using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Outstanding;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared.Helpers;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.Outstanding;

public class OutstandingViewModel
{
    public OutstandingViewModel(
        OutstandingQuery message,
        ReportData<OutstandingModel> reportData,
        ProductMultiSelectionViewModel productViewModel,
        BreadcrumbViewModel breadcrumbLevel,
        bool isMobile)
    {
        CustId = message.CustId;
        CustName = message.CustName;
        CustLevel = message.CustLevel;
        ReportLevel = CustLevel.GetDirectDownLineLevel();
        PageHeader = new PageHeaderViewModel(
            Title: ReportLevel.IsMasterLevel()
                ? Resources.MasterOutstanding
                : ReportLevel.IsAgentLevel() ? Resources.AgentOutstanding : Resources.MemberOutstanding,
            IsFilterVisible: message.IsFilterVisible,
            BreadcrumbLevel: breadcrumbLevel);
        ProductViewModel = productViewModel;
        ReportData = reportData;
        SortPropertyName = message.SortPropertyName;
        DescendingSort = message.DescendingSort;
        IsMobileView = isMobile;
        DisplayOutstandingLevels = GetDisplayOutstandingLevels(CustLevel.Value, message.ViewerLevelId).ToList();
        DisplayOutstandingLevelsJson = JsonHelper.ToJson(DisplayOutstandingLevels);
    }

    public int CustId { get; }

    public string CustName { get; }

    public UserLevel CustLevel { get; }

    public UserLevel ReportLevel { get; }

    public PageHeaderViewModel PageHeader { get; }

    public ProductMultiSelectionViewModel ProductViewModel { get; }

    public ReportData<OutstandingModel> ReportData { get; }

    public IList<UserLevel> DisplayOutstandingLevels { get; }

    public string DisplayOutstandingLevelsJson { get; }

    public string SortPropertyName { get; }

    public byte DescendingSort { get; }

    public bool IsMobileView { get; }

    private IEnumerable<UserLevel> GetDisplayOutstandingLevels(byte custLevel, byte viewerLevel)
    {
        yield return UserLevel.Member;

        for (byte level = custLevel; level <= viewerLevel; level++)
        {
            yield return Enumeration.FromValue<UserLevel>(level);
        }
    }

    public string GetBetListUrl(int custId, string custName)
    {
        var queryString = QueryString.Create(
        [
            KeyValuePair.Create(nameof(custId), custId.ToString()),
            KeyValuePair.Create(nameof(custName), custName),
            KeyValuePair.Create("GetAll", "true"),
            KeyValuePair.Create("ProductIds", ProductViewModel.UserSelectedProductIds)
        ]);

        return $"{BetListPath}{queryString}";
    }

    private static string BetListPath = MicroSites.TicketUrl("~/RunningBetList/Index");
}