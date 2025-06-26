
namespace AlphaPlus.Settings.Site.Features.Customer.ViewModels;
using Fanex.Customer.Client.Models;

public class UpdateInfoViewModel
{
    public UpdateInfoViewModel(
         CustomerInfo customerInfo,
         string cdnUrl,
         string cashServiceSiteUrl,
         string settingSiteUrl,
         string assetsVersion)
    {
        CustomerInfo = customerInfo;
        CdnUrl = cdnUrl;
        CashServiceSiteUrl = cashServiceSiteUrl;
        SettingSiteUrl = settingSiteUrl;
        AssetsVersion = assetsVersion;
    }
    public string CdnUrl { get; }

    public string CashServiceSiteUrl { get; }

    public string SettingSiteUrl { get; }

    public string AssetsVersion { get; }
    public CustomerInfo CustomerInfo { get; set; }

}