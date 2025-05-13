using AlphaPlus.CashService.Site.Features.Shared.Requests;
using AlphaPlus.Core.Models;

namespace AlphaPlus.CashService.Site.Features.Payments.Requests;

public class InsertPaymentMethodRequest : BaseActionRequest
{
    public int ProviderId { get; set; }
    public int TransactionType { get; set; }
    public int Method { get; set; }

    public string APIMethodKey { get; set; } = string.Empty;
    public string MerchantID { get; set; } = string.Empty;
    public string AppSecret { get; set; } = string.Empty;
}