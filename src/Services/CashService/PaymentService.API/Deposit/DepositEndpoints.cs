namespace AlphaPlus.PaymentService.API.Deposit;

public static class DepositEndpoints
{
    public static void RegisterDepositEndpoints(this WebApplication app)
    {
        app.MapPost("/api/payment/deposit/manual", ManualDeposit)
            .Produces<IEnumerable<int>>()
            .WithTags("Manual Deposit");
    }

    static async Task<IResult> ManualDeposit()
    {
        return Results.Ok();
    }
}