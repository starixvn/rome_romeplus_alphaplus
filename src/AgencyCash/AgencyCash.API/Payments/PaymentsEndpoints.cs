using AgencyCash.API.Payments.Models;
using AgencyCash.API.Payments.Requests;
using AgencyCash.API.Payments.Services;
using AgencyCash.API.Shared.Helpers;
using AgencyCash.API.Shared.Models;

namespace AgencyCash.API.Payments;

public static class PaymentsEndpoints
{
    public static void MapPaymentsEndpoints(this WebApplication app)
    {
        var deposit = app.MapGroup("/api/deposit");
        deposit
            .MapGet("/limit", GetLimitDeposit)
            .AddEndpointFilter<ModelValidationFilter<GetLimitDepositRequest>>()
            .Produces<LimitDeposit>()
            .WithTags("Deposit");
        deposit
            .MapPost("/manual", ManualDeposit)
            .AddEndpointFilter<ModelValidationFilter<ManualDepositRequest>>()
            .Produces<PaymentResult>()
            .WithTags("Deposit");
        deposit
            .MapPost("/auto", AutoDeposit)
            .AddEndpointFilter<ModelValidationFilter<AutoDepositRequest>>()
            .Produces<PaymentResult>()
            .WithTags("Deposit");
    }

    private static async Task<IResult> GetLimitDeposit([AsParameters] GetLimitDepositRequest request, IDepositService depositService)
    {
        if (request is null)
        {
            return TypedResults.BadRequest(new Result(StatusCodes.Status400BadRequest, "Request cannot be null."));
        }

        return TypedResults.Ok(await Task.FromResult(new LimitDeposit(1, 1000)));
    }

    private static async Task<IResult> ManualDeposit(ManualDepositRequest request, IDepositService depositService)
    {
        if (request is null)
        {
            return TypedResults.BadRequest(new Result(StatusCodes.Status400BadRequest, "Request cannot be null."));
        }

        return TypedResults.Ok(await depositService.Manual(request));
    }

    private static async Task<IResult> AutoDeposit(AutoDepositRequest request, IDepositService depositService)
    {
        var (isValid, errorMessage) = request.IsValid();

        if (!isValid)
        {
            return TypedResults.BadRequest(new Result(StatusCodes.Status400BadRequest, errorMessage));
        }

        throw new NotImplementedException("Auto deposit functionality is not implemented yet.");
    }
}