using AlphaPlus.Reports.Features.Outstanding;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared.Helpers;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;

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
        ReportLevel = CustLevel.GetDirectDownlineLevel();
        PageHeader = new PageHeaderViewModel(
            Title: ReportLevel.IsMasterLevel()
                ? Resources.MasterOutstanding
                : ReportLevel.IsAgentLevel() ? Resources.AgentOutstanding : Resources.MemberOutstanding,
            IsFilterVisible: message.IsFilterVisible,
            BreadcrumbLevel: breadcrumbLevel);
        ProductViewModel = productViewModel;
        ReportData = reportData;
        SortPropertyName = message.SortPropertyName;
        IsDescendingSort = message.IsDescendingSort;
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

    public bool IsDescendingSort { get; }

    public bool IsMobileView { get; }

    private IEnumerable<UserLevel> GetDisplayOutstandingLevels(byte custLevel, byte viewerLevel)
    {
        yield return UserLevel.Member;

        for (var level = custLevel; level <= viewerLevel; level++)
        {
            yield return Enumeration.FromValue<UserLevel>(level);
        }
    }

    // protected override IEnumerable<KeyValuePair<string, string>> GetDownlineLinkQueryParams(OutstandingModel model)
    //     => [
    //         KeyValuePair.Create(nameof(model.CustId), model.CustId.ToString()),
    //         KeyValuePair.Create(nameof(model.CustName), model.CustName),
    //         KeyValuePair.Create(nameof(ProductMultiSelectionViewModel.UserSelectedProductIds), ProductMultiSelectionViewModel.UserSelectedProductIds),
    //         KeyValuePair.Create(nameof(SortPropertyName), SortPropertyName),
    //         KeyValuePair.Create(nameof(IsDescendingSort), IsDescendingSort.ToString()),
    //         KeyValuePair.Create(nameof(IsFilterVisible), IsFilterVisible.ToString())
    //     ];

    // protected override IEnumerable<KeyValuePair<string, string>> GetBetListLinkQueryParams(OutstandingModel model)
    //     => [
    //         KeyValuePair.Create(nameof(model.CustId), model.CustId.ToString()),
    //         KeyValuePair.Create(nameof(model.CustName), model.CustName),
    //         KeyValuePair.Create("GetAll", "True"),
    //         KeyValuePair.Create("ProductIds", ProductMultiSelectionViewModel.UserSelectedProductIds)];

    // private string GetSortedProperty(string sortedProperty)
    // {
    //     if (string.IsNullOrWhiteSpace(sortedProperty))
    //     {
    //         return DefaultVariables.SortPropertyName;
    //     }

    //     var sortableColumns = DisplayOutstandingLevels
    //         .Select(level => (level.DisplayName + "Outstanding").ToUpperInvariant())
    //         .Concat([CustNamePropertyName])
    //         .ToList();

    //     return sortableColumns.Contains(sortedProperty.ToUpperInvariant())
    //         ? sortedProperty
    //         : DefaultVariables.SortPropertyName;
    // }
}