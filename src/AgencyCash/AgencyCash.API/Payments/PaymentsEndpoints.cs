using AgencyCash.API.Shared.Models;

namespace AgencyCash.API.Payments;

public static class PaymentsEndpoints
{
    public static void MapPaymentsEndpoints(this WebApplication app)
    {
        var payments = app.MapGroup("/api/payments");

        payments.MapPost("/api/deposit/manual", ManualDeposit)
            .Produces<Result>()
            .WithTags("Payments")
            .WithName("Manual Deposit");
    }

    public static async Task<IResult> ManualDeposit()
    {
        var result = new Result(
            0,
            "Manual deposit endpoint hit successfully."
        );
        return Results.Ok(result);
    }
}