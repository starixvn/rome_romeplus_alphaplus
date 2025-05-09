using AlphaPlus.PaymentService.API.Shared;

namespace AlphaPlus.PaymentService.API.U2Pay;

public class U2PayPaymentService(HttpClient client, Fanex.Logging.ILogger logger) : PaymentServiceBase(client, logger), IPaymentService
{
    public string Deposit()
    {
        return "Deposit method called in U2PayPaymentService.";
    }

    public string Withdraw()
    {
        return "Withdraw method called in U2PayPaymentService.";
    }
}
