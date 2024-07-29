using AlphaPlus.Settings.MemberInfo.Domain.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Features.Shared.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using AlphaPlus.Settings.MemberInfo.Domain.Features.Commission;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Configurations;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.Commission;

public class CommissionViewModel(
    CommissionQuery message,
    CommissionReport reportData,
    IEnumerable<Product> products = null,
    IEnumerable<CustInfo> masters = null,
    IEnumerable<CustInfo> agents = null)
{
    public bool IsAgentLevel { get; } = message.ActorLevel.IsAgentLevel();

    public bool IsShowGroup { get; } = message.Id == ProductIdConstants.Sportsbook;

    public bool IsShowDoubleComm { get; } = message.IsShowDoubleComm;

    public IEnumerable<SelectListItem> Products
        => products?.Select(p => new SelectListItem(p.Name, p.Id.ToString(), p.Id == message.Id));

    public IEnumerable<SelectListItem> Masters
        => masters?.Select(c => new SelectListItem(c.CustName, c.CustId.ToString()));

    public IEnumerable<SelectListItem> Agents
        => agents?.Select(c => new SelectListItem(c.CustName, c.CustId.ToString()));

    public CommissionReport ReportData { get; } = reportData;

    public IDictionary<string, string> GetCommHintParams()
        => new Dictionary<string, string>
        {
            ["tabindex"] = "3",
            ["theme"] = string.IsNullOrWhiteSpace(message.DarkModeName) ? string.Empty : "dark",
            ["cusId"] = message.ViewerId.ToString(),
            ["roleId"] = message.ViewerLevelId.ToString(),
            ["site"] = message.Site,
            ["siteLabel"] = AppSettings.SiteLabel,
            ["lng"] = message.Language
        };
}