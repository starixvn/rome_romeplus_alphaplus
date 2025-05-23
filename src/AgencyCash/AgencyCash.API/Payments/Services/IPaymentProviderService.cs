using AgencyCash.API.Payments.Models;
using AgencyCash.API.Shared.Models;

namespace AgencyCash.API.Payments.Services;

public interface IPaymentProviderService
{
    PaymentProvider Provider { get; }
    Task<Result> ProcessWithdrawal(string accountNumber, decimal amount, string currency, bool isAutoApprove);
    Task<Result> ProcessDeposit(string accountNumber, decimal amount, string currency);
    Task<decimal> CalculateFee(decimal amount);
    Task<bool> ValidateAccount(string accountNumber);
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