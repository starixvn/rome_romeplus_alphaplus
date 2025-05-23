using AgencyCash.API.Payments.Models;
using AgencyCash.API.Payments.Requests;
using AgencyCash.API.Payments.Services;
using AgencyCash.API.Shared.Models;

namespace AgencyCash.API.Payments;

public static class PaymentsEndpoints
{
    public static void MapPaymentsEndpoints(this WebApplication app)
    {
        var payments = app.MapGroup("/api/payments");

        var deposit = payments.MapGroup("/deposit");
        deposit.MapGet("/methods", GetDepositMethods)
            .Produces<IEnumerable<DepositMethod>>()
            .WithTags("Payments")
            .WithName("Get Deposit Methods");

        deposit.MapPost("/manual", ManualDeposit)
            .Produces<Result>()
            .WithTags("Payments")
            .WithName("Manual Deposit");

        var withdrawal = payments.MapGroup("/withdraw");
        withdrawal.MapPost("/manual", ManualWithdrawal)
            .Produces<Result>()
            .WithTags("Payments")
            .WithName("Manual Withdrawal");

        withdrawal.MapPost("/auto", AutoWithdrawal)
            .Produces<Result>()
            .WithTags("Payments")
            .WithName("Automatic Withdrawal");
    }

    private static async Task<IResult> GetDepositMethods(int agentId, IDepositService depositService)
        => Results.Ok(await depositService.GetDepositMethods(agentId));

    private static async Task<IResult> ManualDeposit(CreateManualDepositRequest request, IDepositService depositService)
        => Results.Ok(await depositService.CreateManualDeposit(request));

    private static async Task<IResult> ManualWithdrawal(CreateManualWithdrawalRequest request, IWithdrawalService withdrawalService)
        => Results.Ok(await withdrawalService.CreateManualWithdrawal(request));

    private static async Task<IResult> AutoWithdrawal(CreateAutoWithdrawalRequest request, IWithdrawalService withdrawalService)
        => Results.Ok(await withdrawalService.CreateAutoWithdrawal(request));
}