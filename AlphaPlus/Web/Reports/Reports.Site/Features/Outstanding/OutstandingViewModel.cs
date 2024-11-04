using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Reports.Features.Outstanding;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared.Helpers;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.Outstanding;

public class OutstandingViewModel(
    OutstandingQuery message,
    ReportData<OutstandingModel> reportData,
    ProductMultiSelectionViewModel productViewModel,
    bool isMobile) : DrillDownableViewModel<OutstandingModel>(
        message,
        new PageHeaderViewModel(
            Title: BuildTitle(message.CustLevel.GetDirectDownLineLevel()),
            IsFilterVisible: message.IsFilterVisible,
            BreadcrumbLevel: BuildBreadcrumbViewModel(message)),
        reportData,
        message.SortPropertyName,
        message.DescendingSort)
{
    private static string BetListPath = MicroSites.TicketUrl("~/RunningBetList/Index");

    public UserLevel ReportLevel { get; } = message.CustLevel.GetDirectDownLineLevel();

    public ProductMultiSelectionViewModel ProductViewModel { get; } = productViewModel;

    public IList<UserLevel> DisplayOutstandingLevels { get; } = GetDisplayOutstandingLevels(message.CustLevel.Value, message.ViewerLevelId).ToList();

    public string DisplayOutstandingLevelsJson => JsonHelper.ToJson(DisplayOutstandingLevels);

    public bool IsMobileView { get; } = isMobile;

    private static string BuildTitle(UserLevel reportTitle)
        => reportTitle.IsMasterLevel()
        ? Resources.MasterOutstanding
        : reportTitle.IsAgentLevel() ? Resources.AgentOutstanding : Resources.MemberOutstanding;

    private static IEnumerable<UserLevel> GetDisplayOutstandingLevels(byte custLevel, byte viewerLevel)
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
}
