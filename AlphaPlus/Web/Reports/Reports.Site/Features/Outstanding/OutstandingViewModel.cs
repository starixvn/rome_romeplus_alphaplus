using AlphaPlus.Reports.Features.Outstanding;
using AlphaPlus.Reports.Features.Shared.Constants;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Reports.Site.Features.Outstanding;

public class OutstandingViewModel
{
    private const string CustNamePropertyName = nameof(OutstandingModel.CustName);

    public OutstandingViewModel(
        OutstandingQuery query,
        IEnumerable<OutstandingModel> outstandingDataSource,
        ProductMultiSelectionViewModel productViewModel,
        IEnumerable<Breadcrumb> breadcrumbs,
        bool isMobile)
    {
        Query = query;
        ReportLevel = Query.CustLevel.GetDirectDownlineLevel();
        for (var level = (byte)(ReportLevel.Value + 1); level <= Query.ViewerLevel.Value; level++)
        {
            DisplayOutstandingLevels.Add(Enumeration.FromValue<UserLevel>(level));
        }

        ProductMultiSelectionViewModel = productViewModel;

        OutstandingDatasource = outstandingDataSource;
        CanSort = OutstandingDatasource.Count() > 1;
        SortPropertyName = CanSort ? GetSortedProperty(query.SortPropertyName) : DefaultVariables.SortPropertyName;
        IsDescendingSort = CanSort && query.IsDescendingSort;

        //BetListPath = "/tickets/RunningBetList/Index";
        IsMobileView = isMobile;
    }

    public OutstandingQuery Query { get; }

    public UserLevel ReportLevel { get; }

    public bool IsFilterVisible => Query.IsFilterVisible;

    public List<UserLevel> DisplayOutstandingLevels { get; } = new();

    public int NumberOfOutstandingColumn => DisplayOutstandingLevels.Count;

    public ProductMultiSelectionViewModel ProductMultiSelectionViewModel { get; }

    public IEnumerable<OutstandingModel> OutstandingDatasource { get; }

    public OutstandingModel Total => OutstandingModel.GetAccumulatedData(OutstandingDatasource);

    public bool CanSort { get; }

    public string SortPropertyName { get; }

    public bool IsDescendingSort { get; }

    public bool IsMobileView { get; }

    public string Title
        => ReportLevel.IsMasterLevel()
        ? Resources.MasterOutstanding
        : ReportLevel.IsAgentLevel()
            ? Resources.AgentOutstanding
            : Resources.MemberOutstanding;

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

    private string GetSortedProperty(string sortedProperty)
    {
        if (string.IsNullOrWhiteSpace(sortedProperty))
        {
            return DefaultVariables.SortPropertyName;
        }

        var sortableColumns = DisplayOutstandingLevels
            .Select(level => (level.DisplayName + "Outstanding").ToUpperInvariant())
            .Concat([CustNamePropertyName])
            .ToList();

        return sortableColumns.Contains(sortedProperty.ToUpperInvariant())
            ? sortedProperty
            : DefaultVariables.SortPropertyName;
    }
}