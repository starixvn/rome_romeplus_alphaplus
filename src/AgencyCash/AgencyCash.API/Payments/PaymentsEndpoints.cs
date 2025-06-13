using AgencyCash.API.Payments.Models;
using AgencyCash.API.Payments.Requests;
using AgencyCash.API.Payments.Services;
using AgencyCash.API.Shared.DataAnnotations;
using AgencyCash.API.Shared.Models;
using Microsoft.AspNetCore.Mvc;

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
        deposit
            .MapPut("/{transId}/status", UpdateDepositStatus)
            .AddEndpointFilter<ModelValidationFilter<UpdateTransactionStatusRequest>>()
            .Produces<Result>();

        var withdrawal = app.MapGroup("/api/withdrawal").WithTags("Withdrawal");
        withdrawal
            .MapPost("/manual", ManualWithdrawal)
            .AddEndpointFilter<ModelValidationFilter<ManualWithdrawalRequest>>()
            .Produces<PaymentResult>();
        withdrawal
            .MapGet("/limit", GetWithdrawalLimitAmount)
            .AddEndpointFilter<ModelValidationFilter<GetWithdrawalLimitRequest>>()
            .Produces<WithdrawalLimitResult>();
        withdrawal
            .MapPut("/{transId}/status", UpdateWithdrawalStatus)
            .AddEndpointFilter<ModelValidationFilter<UpdateTransactionStatusRequest>>()
            .Produces<Result>();
    }

    private static async Task<IResult> GetDepositLimit([AsParameters] GetDepositLimitRequest request, IDepositService depositService)
    {
        if (request == null)
        {
            return BadRequest(nameof(request), $"{nameof(request)} cannot be null.");
        }

        return TypedResults.Ok(await depositService.GetLimit(request));
    }

    private static async Task<IResult> ManualDeposit([FromBody] ManualDepositRequest request, IDepositService depositService)
    {
        if (request == null)
        {
            return BadRequest(nameof(request), $"{nameof(request)} cannot be null.");
        }

        var paySlipValidationResult = request.ValidatePaySlip();
        if (paySlipValidationResult != null)
        {
            return TypedResults.BadRequest(paySlipValidationResult);
        }

        return TypedResults.Ok(await depositService.Manual(request));
    }

    private static async Task<IResult> AutoDeposit([FromBody] AutoDepositRequest request, IDepositService depositService)
    {
        if (request == null)
        {
            return BadRequest(nameof(request), $"{nameof(request)} cannot be null.");
        }

        return TypedResults.Ok(await depositService.Auto(request));
    }

    private static async Task<IResult> UpdateDepositStatus(
        long transId,
        [FromBody]
        UpdateTransactionStatusRequest request,
        IDepositService depositService)
    {
        if (request == null)
        {
            return BadRequest(nameof(request), $"{nameof(request)} cannot be null.");
        }

        if (transId <= 0)
        {
            return BadRequest(nameof(transId), $"{nameof(transId)} must be a positive number.");
        }

        return TypedResults.Ok(await depositService.UpdateTransactionStatus(transId, request));
    }

    private static async Task<IResult> ManualWithdrawal([FromBody] ManualWithdrawalRequest request, IWithdrawalService withdrawalService)
    {
        if (request is null)
        {
            return BadRequest(nameof(request), $"{nameof(request)} cannot be null.");
        }

        var result = await withdrawalService.CreateManualWithdrawal(request);
        return TypedResults.Ok(result);
    }

    private static async Task<IResult> GetWithdrawalLimitAmount([AsParameters] GetWithdrawalLimitRequest request, IWithdrawalService withdrawalService)
    {
        if (request is null)
        {
            return BadRequest(nameof(request), $"{nameof(request)} cannot be null.");
        }

        return TypedResults.Ok(await withdrawalService.GetWithdrawalLimitAmount(request));
    }

    private static async Task<IResult> UpdateWithdrawalStatus(
        long transId,
        [FromBody]
        UpdateTransactionStatusRequest request,
        IWithdrawalService withdrawalService)
    {
        if (request == null)
        {
            return BadRequest(nameof(request), $"{nameof(request)} cannot be null.");
        }

        if (transId <= 0)
        {
            return BadRequest(nameof(transId), $"{nameof(transId)} must be a positive number.");
        }

        return TypedResults.Ok(await withdrawalService.UpdateTransactionStatus(transId, request));
    }

    private static IResult BadRequest(string key, string error)
        => TypedResults.BadRequest(new ModelValidationResult(key, error));
}