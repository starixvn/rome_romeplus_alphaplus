using AgencyCash.API.Payments.Models;
using AgencyCash.API.Payments.Services;
using AgencyCash.API.Shared.Models;

namespace AgencyCash.API.Payments;

public static class PaymentsEndpoints
{
    public static void MapPaymentsEndpoints(this WebApplication app)
    {
        var payments = app.MapGroup("/api/payments/deposit");

        payments.MapGet("/methods", GetDepositMethods)
            .Produces<IEnumerable<DepositMethod>>()
            .WithTags("Payments")
            .WithName("Get Deposit Methods");

        payments.MapPost("/manual", ManualDeposit)
            .Produces<Result>()
            .WithTags("Payments")
            .WithName("Manual Deposit");
    }

    private static async Task<IResult> GetDepositMethods(int agentId, IDepositService depositService)
    {
        var depositMethods = await depositService.GetDepositMethods(agentId);
        return TypedResults.Ok(depositMethods);
    }

    private static async Task<IResult> ManualDeposit()
    {
        var result = new Result(
            0,
            "Manual deposit endpoint hit successfully."
        );
        return Results.Ok(result);
    }
}