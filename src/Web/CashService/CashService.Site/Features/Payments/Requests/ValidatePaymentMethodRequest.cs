namespace AlphaPlus.CashService.Site.Features.Payments.Requests;

public class ValidatePaymentMethodRequest
{
    public int AgentId { get; set; }
    public int ProviderId { get; set; }
    public int TransactionType { get; set; }
    public int Method { get; set; }
    public string APIMethodKey { get; set; }
    public string MerchantID { get; set; }
    public string AppSecret { get; set; }
} 