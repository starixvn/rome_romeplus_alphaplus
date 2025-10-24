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
        message.LevelType,
        new PageHeaderViewModel(
            Title: BuildTitle(message.CustLevel.GetDirectDownLineLevel()),
            IsFilterVisible: message.IsFilterVisible,
            BreadcrumbLevel: BuildBreadcrumbViewModel(message)),
        reportData,
        message.SortPropertyName,
        message.DescendingSort,
        betListPath: "~/RunningBetList/Index")
{
    public UserLevel ReportLevel { get; } = message.CustLevel.GetDirectDownLineLevel();

    public ProductMultiSelectionViewModel ProductViewModel { get; } = productViewModel;

    public IList<UserLevel> DisplayOutstandingLevels { get; } = GetDisplayOutstandingLevels(message.CustLevel.Value, message.ViewerLevelId).ToList();

    public string DisplayOutstandingLevelsJson => JsonHelper.ToJson(DisplayOutstandingLevels);

    public bool IsMobileView { get; } = isMobile;

    public bool TotalHasToWinValue => ReportData.DataSource.Any(i => i.ToWin.HasValue);

    public string GetSMARiskText 
        => message.ViewerLevel.IsAgentLevel() ? Resources.AgentRisk :
        message.ViewerLevel.IsMasterLevel() ? Resources.MasterRisk :
        Resources.SuperRisk;

    public byte ViewerLevel => message.ViewerLevelId;

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

    protected override IDictionary<string, string> AddMoreBetListParams(IDictionary<string, string> queryParams)
    {
        queryParams["GetAll"] = "true";
        queryParams["ProductIds"] = ProductViewModel.UserSelectedProductIds;

        return queryParams;
    }

    public bool IsNegative(decimal? cellValue) 
    {
        if (!cellValue.HasValue) 
        {
            return false;
        }
        var rounded = Math.Round(cellValue.Value, 2, MidpointRounding.AwayFromZero);
        return rounded < 0;
    }
}
