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
            .MapGet("/limit", GetDepositLimit)
            .AddEndpointFilter<ModelValidationFilter<GetDepositLimitRequest>>()
            .Produces<DepositLimit>();
        deposit
            .MapPost("/manual", ManualDeposit)
            .AddEndpointFilter<ModelValidationFilter<ManualDepositRequest>>()
            .Produces<PaymentResult>();
        deposit
            .MapPost("/auto", AutoDeposit)
            .AddEndpointFilter<ModelValidationFilter<AutoDepositRequest>>()
            .Produces<PaymentResult>();

        var withdrawal = app.MapGroup("/api/withdrawal").WithTags("Withdrawal");
        withdrawal
            .MapPost("/manual", ManualWithdrawal)
            .AddEndpointFilter<ModelValidationFilter<ManualWithdrawalRequest>>()
            .Produces<Result>();
    }

    private static async Task<IResult> GetDepositLimit([AsParameters] GetDepositLimitRequest request, IDepositService depositService)
    {
        if (request == null)
        {
            return BadRequest("Request cannot be null.");
        }

        return TypedResults.Ok(await depositService.GetLimit(request));
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

    private static async Task<IResult> ManualWithdrawal(
        ManualWithdrawalRequest request,
        IWithdrawalService withdrawalService)
    {
        if (request is null || !request.IsValid())
        {
            return BadRequest("Request parameters is invalid.");
        }

        // Convert to PaymentService.API request
        var paymentServiceRequest = new ManualWithdrawalRequest
        {
            CustId = request.CustId,
            AgentId = request.AgentId,
            PaymentFrom = request.PaymentFrom,
            PaymentTo = request.PaymentTo,
            Amount = request.Amount,
            IP = request.IP
        };

        var result = await withdrawalService.CreateManualWithdrawal(paymentServiceRequest);
        return TypedResults.Ok(result);
    }

    private static IResult BadRequest(string error)
        => TypedResults.BadRequest(new Result(StatusCodes.Status400BadRequest, error));
}