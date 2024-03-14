using AlphaPlus.Reports.Features.IPMatching;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Features.Shared.Models;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Reports.Site.Features.IPMatching;

public class IPMatchingViewModel(IPMatchingQuery message, ReportData<IPMatchingModel> reportData)
{
    public string Date { get; } = message.Date.ToShortDateWithSlash();

    public string Today { get; } = DateTime.Today.ToShortDateWithSlash();

    public string MinDate { get; } = message.MinDate.ToShortDateWithSlash();

    public PageHeaderViewModel PageHeader { get; }
        = new PageHeaderViewModel(Resources.IPMatching, IsFilterVisible: message.IsFilterVisible);

    public IEnumerable<SelectListItem> MemberCounts
        => new int[] { 2, 3, 4, 5, 8, 10, 15, 20, 30, 50, 100 }
        .Select(item => new SelectListItem($">= {item}", item.ToString(), item == message.MemberCount));

    public ReportData<IPMatchingModel> ReportData { get; } = reportData;

    public IDictionary<string, string> GetExcelParams()
        => new Dictionary<string, string>
        {
            [nameof(Date)] = Date,
            [nameof(message.MemberCount)] = message.MemberCount.ToString()
        };
}
