namespace AlphaPlus.PaymentService.API.Payments.Models;

public class PaymentProviderConfig
{
    public U2Pay U2Pay { get; set; }
    public DirePay DirePay { get; set; }
}

public class U2Pay : ProviderBase
{
}

public class DirePay : ProviderBase
{
}

public class ProviderBase
{
    public string MerchantName { get; set; }
    public string MerchantNo { get; set; }
    public string ApiKey { get; set; }
    public string ApiSecret { get; set; }
    public string BaseUrl { get; set; }
    public bool IsEnabled { get; set; }
    public decimal MinAmount { get; set; }
    public decimal MaxAmount { get; set; }
    public decimal Fee { get; set; }
    public string FeeType { get; set; }
}
