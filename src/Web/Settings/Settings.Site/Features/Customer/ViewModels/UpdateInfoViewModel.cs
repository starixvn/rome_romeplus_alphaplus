using Fanex.Customer.Client.Models;

namespace AlphaPlus.Settings.Site.Features.Customer.ViewModels;
public class UpdateInfoViewModel
{
    public UpdateInfoViewModel(
         CustomerInfo customerInfo,
         string assetsVersion,
         int custId,
         bool isCashMember)
    {
        CustomerInfo = customerInfo;
        AssetsVersion = assetsVersion;
        CustId = custId;
        IsCashMember = isCashMember;
    }

    public string CdnUrl { get; }

    public string CashServiceSiteUrl { get; }

    public string SettingSiteUrl { get; }

    public string AssetsVersion { get; }

    public bool IsCashMember { get; }

    public int CustId { get; }

    public CustomerInfo CustomerInfo { get; set; }

}