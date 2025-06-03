using AlphaPlus.CashService.API.Features.Payments.Models;

namespace AlphaPlus.CashService.API.Features.Payments.ViewModels
{
    public class AddPaymentMethodViewModel
    {
       public IEnumerable<PaymentMethodModel> PaymentMethods { get; set; }
       public ChannelContactModel ChannelContact { get; set; }
    }
} 