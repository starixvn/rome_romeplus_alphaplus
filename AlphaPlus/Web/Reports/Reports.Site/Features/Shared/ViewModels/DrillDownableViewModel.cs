using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.Report;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared.Helpers;
using AlphaPlus.Reports.Site.Features.Shared.Requests;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Shared.AppResources.Common;

namespace AlphaPlus.Reports.Site.Features.Shared.ViewModels;

public abstract class DrillDownableViewModel<T> where T : class
{
    private string betListPath;

    protected DrillDownableViewModel(
        BaseQuery message,
        byte levelType,
        PageHeaderViewModel pageHeader,
        ReportData<T> reportData,
        string sortPropertyName,
        byte descendingSort,
        string betListPath = null)
    {
        CustId = message.CustId;
        CustName = message.CustName;
        CustLevel = message.CustLevel;
        LevelType = levelType;
        LevelTypeValue = CustLevel.IsSuperLevel() ? CommonResx.MasterAgent : Resources.Agent;
        ShowLevelType = CustLevel.IsMasterOrSuperLevel();
        PageHeader = pageHeader;
        ReportData = reportData;
        SortPropertyName = sortPropertyName;
        DescendingSort = descendingSort;

        if (!string.IsNullOrWhiteSpace(betListPath))
        {
            this.betListPath = MicroSites.TicketUrl(betListPath);
        }
    }

    public int CustId { get; }

    public string CustName { get; }
    
    public byte DownlineLevelId { get; } = (byte)(message.CustLevelId - 1);


    public UserLevel CustLevel { get; }

    public byte LevelType { get; }

    public string LevelTypeValue { get; }

    public bool ShowLevelType { get; }

    public PageHeaderViewModel PageHeader { get; }

    public ReportData<T> ReportData { get; }

    public string SortPropertyName { get; }

    public byte DescendingSort { get; }

    protected static BreadcrumbViewModel BuildBreadcrumbViewModel(BaseQuery message)
    {
        var breadcrumbs = BreadcrumbHelper.BuildBreadcrumbs(message);

        return new BreadcrumbViewModel(breadcrumbs, JsonHelper.ToJson(breadcrumbs));
    }

    public IDictionary<string, string> BuildExcelParams()
    {
        var queryParams = new Dictionary<string, string>
        {
            [nameof(CustId)] = CustId.ToString(),
            [nameof(CustName)] = CustName,
            ["CustLevelId"] = CustLevel.Value.ToString(),
            [nameof(LevelType)] = LevelType.ToString()
        };

        return AddMoreExcelParams(queryParams);
    }

    protected virtual IDictionary<string, string> AddMoreExcelParams(IDictionary<string, string> queryParams) => queryParams;

    public string BuildBetListUrl(int custId, string custName)
    {
        var queryParams = new Dictionary<string, string>
        {
            [nameof(custId)] = custId.ToString(),
            [nameof(custName)] = custName,
        };

        return $"{betListPath}{QueryString.Create(AddMoreBetListParams(queryParams))}";
    }

    protected virtual IDictionary<string, string> AddMoreBetListParams(IDictionary<string, string> queryParams) => queryParams;
}
