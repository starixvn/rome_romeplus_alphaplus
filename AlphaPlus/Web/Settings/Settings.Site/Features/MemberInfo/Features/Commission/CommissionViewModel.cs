using AlphaPlus.Settings.MemberInfo.Domain.Models;
using AlphaPlus.Settings.MemberInfo.Domain.Features.Shared.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using AlphaPlus.Settings.MemberInfo.Domain.Features.Commission;
using AlphaPlus.Settings.MemberInfo.Domain.Configurations;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.Commission;

public class CommissionViewModel(
    CommissionQuery message,
    CommissionReport reportData,
    IEnumerable<Product> products = null,
    IEnumerable<CustInfo> masters = null,
    IEnumerable<CustInfo> agents = null,
    int masterId = 0,
    int recommendId = 0) : CommDataViewModel(message, reportData)
{
    public IEnumerable<SelectListItem> Products
        => products?.Select(p => new SelectListItem(p.Name, p.Id.ToString(), p.Id == message.ProductId));

    public IEnumerable<SelectListItem> Masters
        => masters?.Select(c => new SelectListItem(c.CustName, c.CustId.ToString(), c.CustId == masterId));

    public IEnumerable<SelectListItem> Agents
        => agents?.Select(c => new SelectListItem(c.CustName, c.CustId.ToString(), c.CustId == recommendId));

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