using AlphaPlus.Core.Extensions;
using AlphaPlus.Reports.Features.CashBillingService.Models;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.WinLoss;

public class CashFeeViewModel(
CashFeeGetDataQuery message,
CashFeeModel cashFeeModel,
IEnumerable<Reports.Features.Shared.Models.Breadcrumb> breadcrumbs)
{
    public PageHeaderViewModel PageHeader { get; } = new PageHeaderViewModel(
        Title: GetCashFeeTitle(),
        IsFilterVisible: false);

    public IEnumerable<Reports.Features.Shared.Models.Breadcrumb> Breadcrumbs { get; } = breadcrumbs;

    public CashFeeModel CashFee { get; } = cashFeeModel;

    private static string GetCashFeeTitle()
    {
        return Resources.CashServiceBilling.ToUpper();
    }

    public IDictionary<string, string> GetExcelParams()
       => new Dictionary<string, string>
       {
           ["CustId"] = message.CustId.ToString(),
           ["FromDate"] = message.FromDate.ToShortDateWithSlash(),
           ["ToDate"] = message.ToDate.ToShortDateWithSlash()
       };
}