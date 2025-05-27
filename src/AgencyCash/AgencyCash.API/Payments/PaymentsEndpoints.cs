using AgencyCash.API.Payments.Models;
using AgencyCash.API.Payments.Requests;
using AgencyCash.API.Payments.Services;
using AgencyCash.API.Shared.DataAnnotations;
using AgencyCash.API.Shared.Helpers;
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
        if (request == null)
        {
            return BadRequest("Request cannot be null.");
        }

        return TypedResults.Ok(await Task.FromResult(new LimitDeposit(1, 1000)));
    }

    private static async Task<IResult> ManualDeposit(ManualDepositRequest request, IDepositService depositService)
    {
        if (request == null)
        {
            return BadRequest("Request cannot be null.");
        }

        const int MaxPaySlipSize = 1024 * 1024;
        request.PaySlipBuffer = Base64StringHelper.ConvertToByte(request.PaySlip);
        if (request.PaySlipBuffer != null && request.PaySlipBuffer.Length > MaxPaySlipSize)
        {
            return BadRequest($"{nameof(request.PaySlip)} cannot exceed 1MB.");
        }


        return TypedResults.Ok(await depositService.Manual(request));
    }

    private static async Task<IResult> AutoDeposit(AutoDepositRequest request, IDepositService depositService)
    {
        if (request == null)
        {
            return BadRequest("Request cannot be null.");
        }

        return TypedResults.Ok(await Task.FromResult(new PaymentResult(0, string.Empty, 123456789)));
    }

    private static IResult BadRequest(string error)
        => TypedResults.BadRequest(new Result(StatusCodes.Status400BadRequest, error));
}