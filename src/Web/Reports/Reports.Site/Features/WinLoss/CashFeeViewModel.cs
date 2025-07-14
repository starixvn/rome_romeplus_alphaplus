using AlphaPlus.Core.Extensions;
using AlphaPlus.Reports.Features.CashBillingService.Models;
using AlphaPlus.Reports.Site.Features.Shared.ViewModels.Components;
using AlphaPlus.Shared.AppResources.Report;

namespace AlphaPlus.Reports.Site.Features.WinLoss;

public class CashFeeViewModel(
CashFeeGetDataQuery message,
CashFeeModel cashFeeModel)
{
    public PageHeaderViewModel PageHeader { get; } = new PageHeaderViewModel(
        Title: GetCashFeeTitle(message),
        HasHideFilterIcon: false);

    public CashFeeModel CashFee { get; } = cashFeeModel;

    private static string GetCashFeeTitle(CashFeeGetDataQuery message)
    {
        return $"{Resources.CashServiceBilling.ToUpper()} {message.FromDate.ToShortDateWithSlash()} --> {message.ToDate.ToShortDateWithSlash()} - {message.CustName}";
    }

    public IDictionary<string, string> GetExcelParams()
       => new Dictionary<string, string>
       {
           ["CustId"] = message.CustId.ToString(),
           ["FromDate"] = message.FromDate.ToShortDateWithSlash(),
           ["ToDate"] = message.ToDate.ToShortDateWithSlash()
       };
}