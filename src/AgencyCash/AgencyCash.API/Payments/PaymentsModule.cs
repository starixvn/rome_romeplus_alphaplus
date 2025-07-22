using System.Diagnostics.CodeAnalysis;
using AgencyCash.API.Payments.Services;

namespace AgencyCash.API.Payments;

[ExcludeFromCodeCoverage(Justification = "This module contains only registration logic and does not require testing.")]
public static class PaymentsModule
{
    public static IServiceCollection RegisterPaymentsModule(this IServiceCollection services)
        => services
        .AddSingleton<IDepositService, DepositService>()
        .AddSingleton<IWithdrawalService, WithdrawalService>()
        .AddSingleton<ITransactionService, TransactionService>();
}