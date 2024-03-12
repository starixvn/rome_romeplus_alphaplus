using AlphaPlus.Reports.Features.IPMatching;
using AlphaPlus.Reports.Features.Shared.Extensions;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Reports.Site.Features.IPMatching;

public class IPMatchingViewModel(IPMatchingQuery message, IEnumerable<IPMatchingModel> reportData, IPMatchingModel total)
{
    public string Date { get; } = message.Date.ToShortDateWithSlash();

    public string MinDate { get; } = message.MinDate.ToShortDateWithSlash();

    public PageHeaderViewModel PageHeader { get; }
        = new PageHeaderViewModel(Resources.IPMatching, IsFilterVisible: message.IsFilterVisible);

    public IEnumerable<SelectListItem> MemberCounts
        => new int[] { 2, 3, 4, 5, 8, 10, 15, 20, 30, 50, 100 }
        .Select(item => new SelectListItem($">= {item}", item.ToString(), item == message.MemberCount));

    public IEnumerable<IPMatchingModel> ReportData { get; } = reportData;

    public IPMatchingModel Total { get; } = total;

    public IDictionary<string, string> GetExcelParams()
        => new Dictionary<string, string>
        {
            [nameof(Date)] = Date,
            [nameof(message.MemberCount)] = message.MemberCount.ToString()
        };
}
