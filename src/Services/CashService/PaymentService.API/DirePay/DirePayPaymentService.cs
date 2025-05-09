using AlphaPlus.PaymentService.API.Shared;

namespace AlphaPlus.PaymentService.API.DirePay;

public class DirePayPaymentService(HttpClient client, Fanex.Logging.ILogger logger) : PaymentServiceBase(client, logger), IPaymentService
{
    public string Deposit()
    {
        return "Deposit method called in DirePayPaymentService.";
    }

    public string Withdraw()
    {
        return "Withdraw method called in DirePayPaymentService.";
    }
}