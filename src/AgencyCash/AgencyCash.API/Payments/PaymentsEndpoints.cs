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

        var transactions = app.MapGroup("/api/transactions").WithTags("Transactions");
        transactions
            .MapPut("/{transId}/status", UpdateTransactionStatus)
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

    private static async Task<IResult> ManualDeposit(ManualDepositRequest request, IDepositService depositService)
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

    private static async Task<IResult> AutoDeposit(AutoDepositRequest request, IDepositService depositService)
    {
        if (request == null)
        {
            return BadRequest(nameof(request), $"{nameof(request)} cannot be null.");
        }

        return TypedResults.Ok(await depositService.Auto(request));
    }

    private static async Task<IResult> ManualWithdrawal(
        ManualWithdrawalRequest request,
        IWithdrawalService withdrawalService)
    {
        if (request is null || !request.IsValid())
        {
            return BadRequest(nameof(request), "Request parameters is invalid.");
        }

        // Convert to PaymentService.API request
        var paymentServiceRequest = new ManualWithdrawalRequest
        {
            CustId = request.CustId,
            AgentId = request.AgentId,
            PaymentTo = request.PaymentTo,
            Amount = request.Amount,
            IP = request.IP
        };

        var result = await withdrawalService.CreateManualWithdrawal(paymentServiceRequest);
        return TypedResults.Ok(result);
    }

    private static async Task<IResult> UpdateTransactionStatus(
        long transId,
        UpdateTransactionStatusRequest request)
    {
        if (request == null)
        {
            return BadRequest(nameof(request), $"{nameof(request)} cannot be null.");
        }

        if (transId <= 0)
        {
            return BadRequest(nameof(transId), $"{nameof(transId)} must be a positive number.");
        }

        return TypedResults.Ok(await Task.FromResult(new Result(0, "Transaction status updated successfully.")));
    }

    private static IResult BadRequest(string key, string error)
        => TypedResults.BadRequest(new ModelValidationResult(key, error));
}