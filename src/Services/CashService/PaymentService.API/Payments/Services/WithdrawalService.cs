using PaymentService.API.Payments.Models;
using PaymentService.API.Payments.Requests;
using PaymentService.API.Shared;
using PaymentService.API.Shared.Models;
using Fanex.Data.Repository;

namespace PaymentService.API.Payments.Services;

public interface IWithdrawalService
{
    Task<Result> ProcessPayout(CreateManualWithdrawalRequest request);
}

public class WithdrawalService : PaymentService, IWithdrawalService
{
    private readonly IEnumerable<IPaymentProviderService> _paymentProviders;
    private readonly PaymentProviderConfig _paymentProviderConfig;

    public WithdrawalService(
        IDynamicRepository repository,
        ISiteDBService siteDBService,
        Fanex.Logging.ILogger logger,
        IEnumerable<IPaymentProviderService> paymentProviders,
        PaymentProviderConfig paymentProviderConfig) 
        : base(repository, siteDBService, logger)
    {
        _paymentProviders = paymentProviders;
        _paymentProviderConfig = paymentProviderConfig;
    }

    public async Task<Result> ProcessPayout(CreateManualWithdrawalRequest request)
    {
        // Process withdrawal through payment provider
        var provider = GetPaymentProvider(request.PaymentFrom);
        if (provider == null)
        {
            return new Result(606, "Invalid payment provider");
        }

        var providerResult = await provider.ProcessPayout(new PayoutRequest{
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

    private IPaymentProviderService GetPaymentProvider(string paymentFrom)
    {
        // TODO: Implement logic to determine provider based on paymentFrom
        // This is a placeholder implementation
        if (paymentFrom.StartsWith("U2"))
        {
            return _paymentProviders.FirstOrDefault(p => p.Provider == PaymentProvider.U2Pay);
        }
        else if (paymentFrom.StartsWith("DIR"))
        {
            return _paymentProviders.FirstOrDefault(p => p.Provider == PaymentProvider.DirePay);
        }
        return null;
    }
} 