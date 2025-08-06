using AlphaPlus.PaymentService.API.Payments.Models;
using AlphaPlus.PaymentService.API.Payments.Requests;
using AlphaPlus.PaymentService.API.Shared.Models;

namespace AlphaPlus.PaymentService.API.Payments.Services;

public interface IPaymentWithProviderService
{
    Task<Result> ProcessPayout(CreateManualWithdrawalRequest request);
}

public class PaymentWithProviderService(
    IPaymentProviderService paymentProvider,
    PaymentProviderConfig paymentProviderConfig) : IPaymentWithProviderService
{
    private readonly IPaymentProviderService _paymentProvider = paymentProvider;
    private readonly PaymentProviderConfig _paymentProviderConfig = paymentProviderConfig;

    public async Task<Result> ProcessPayout(CreateManualWithdrawalRequest request)
    {
        // Process withdrawal through payment provider
        var provider = _paymentProvider;
        if (provider == null)
        {
            return new Result(606, "Invalid payment provider");
        }

        var providerResult = await provider.ProcessPayout(new PayoutRequest
        {
            MerchantNo = request.MerchantNo,
            OrderNo = request.PaymentTo,
            Amount = (int)request.Amount,
            Name = request.PaymentTo,
            BankName = request.PaymentTo,
            BankAccount = request.PaymentTo,
            BankBranch = request.PaymentTo,
            Memo = request.PaymentTo,
            Mobile = request.PaymentTo,
            Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            NotifyUrl = request.PaymentTo,
            ReverseUrl = request.PaymentTo,
            Extra = request.PaymentTo,
            Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
        },
            _paymentProviderConfig);

        return providerResult;
    }
}