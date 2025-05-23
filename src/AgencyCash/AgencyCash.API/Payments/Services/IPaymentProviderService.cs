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
} 