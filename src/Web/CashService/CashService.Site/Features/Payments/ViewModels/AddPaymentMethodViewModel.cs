using AlphaPlus.CashService.Site.Features.Payments.Models;

namespace AlphaPlus.CashService.Site.Features.Payments.ViewModels
{
    public class AddPaymentMethodViewModel
    {
       public IEnumerable<PaymentMethodModel> PaymentMethods { get; set; }
       public ChannelContactModel ChannelContact { get; set; }
    }
} 