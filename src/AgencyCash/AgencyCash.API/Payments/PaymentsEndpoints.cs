using AgencyCash.API.Payments.Models;
using AgencyCash.API.Payments.Requests;
using AgencyCash.API.Payments.Services;
using AgencyCash.API.Shared.DataAnnotations;
using AgencyCash.API.Shared.Models;

namespace AgencyCash.API.Payments;

public static class PaymentsEndpoints
{
    public static void MapPaymentsEndpoints(this WebApplication app)
    {
        var deposit = app.MapGroup("/api/deposit").WithTags("Deposit");
        deposit
            .MapGet("/limit", GetLimitDeposit)
            .AddEndpointFilter<ModelValidationFilter<GetLimitDepositRequest>>()
            .Produces<LimitDeposit>();
        deposit
            .MapPost("/manual", ManualDeposit)
            .AddEndpointFilter<ModelValidationFilter<ManualDepositRequest>>()
            .Produces<PaymentResult>();
        deposit
            .MapPost("/auto", AutoDeposit)
            .AddEndpointFilter<ModelValidationFilter<AutoDepositRequest>>()
            .Produces<PaymentResult>();
    }

    private static async Task<IResult> GetLimitDeposit([AsParameters] GetLimitDepositRequest request, IDepositService depositService)
    {
        if (request is null)
        {
            return NullBadRequest();
        }

        return TypedResults.Ok(await Task.FromResult(new LimitDeposit(1, 1000)));
    }

    private static async Task<IResult> ManualDeposit(ManualDepositRequest request, IDepositService depositService)
    {
        if (request is null)
        {
            return NullBadRequest();
        }

        return TypedResults.Ok(await depositService.Manual(request));
    }

    private static async Task<IResult> AutoDeposit(AutoDepositRequest request, IDepositService depositService)
    {
        if (request is null)
        {
            return NullBadRequest();
        }

        return TypedResults.Ok(await Task.FromResult(new PaymentResult(0, string.Empty, 123456789)));
    }

    private static IResult NullBadRequest()
        => TypedResults.BadRequest(new Result(StatusCodes.Status400BadRequest, "Request cannot be null."));
}