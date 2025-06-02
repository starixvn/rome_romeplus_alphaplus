using AlphaPlus.CashService.API.Features.Shared.Requests;

namespace AlphaPlus.CashService.API.Features.Payments.Requests;

public class InsertPaymentMethodRequest : BaseActionRequest
{
    public int ProviderId { get; set; }
    public int TransactionType { get; set; }
    public int Method { get; set; }

    public string APIMethodKey { get; set; } = string.Empty;
    public string MerchantID { get; set; } = string.Empty;
    public string AppSecret { get; set; } = string.Empty;

    public override bool IsValid()
        => base.IsValid() && ProviderId > 0;
} 