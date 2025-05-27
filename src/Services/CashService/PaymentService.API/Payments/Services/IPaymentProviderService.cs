using PaymentService.API.Payments.Models;
using PaymentService.API.Payments.Requests;
using PaymentService.API.Shared.Models;

namespace PaymentService.API.Payments.Services;

public interface IPaymentProviderService
{
    PaymentProvider Provider { get; }
    Task<Result> ProcessDeposit(string accountNumber, decimal amount, string currency);
    Task<decimal> CalculateFee(decimal amount);
    Task<bool> ValidateAccount(string accountNumber);
    Task<Result> ProcessPayout(PayoutRequest request, PaymentProviderConfig config);
    Task<Result<IPayoutStatusResponse>> CheckPayoutStatusAsync(string tradeNo, string orderNo, PaymentProviderConfig config);
    Task<Result<IPayoutBalanceResponse>> GetBalanceAsync(PaymentProviderConfig config);
}

public interface IPayoutStatusResponse
{
    int StatusCode { get; }
    string Message { get; }
    string Status { get; }
    string TransactionId { get; }
    string OrderId { get; }
}

public interface IPayoutBalanceResponse
{
    int StatusCode { get; }
    string Message { get; }
    decimal AvailableBalance { get; }
    string Currency { get; }
    string LastUpdated { get; }
} 