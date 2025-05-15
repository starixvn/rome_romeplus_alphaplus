using AlphaPlus.CashService.Site.Features.Payments.Models;

namespace AlphaPlus.CashService.Site.Features.Payments.ViewModels
{
    public class ProviderSettingsViewModel : PaymentBaseViewModel
    {
        public ProviderModel Provider { get; set; }
        public AddPaymentMethodViewModel AddPaymentMethodViewModel { get; set; }
    }
} 