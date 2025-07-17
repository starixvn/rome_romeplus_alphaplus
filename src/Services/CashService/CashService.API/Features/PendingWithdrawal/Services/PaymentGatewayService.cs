namespace AlphaPlus.CashService.API.Features.PendingWithdrawal.Services;

using System.Diagnostics;
using AlphaPlus.CashService.API.Features.Shared.Models;
using Helpers;
using Models.PaymentGateway;
using Newtonsoft.Json;
using Shared.Criteria;
using Shared.Services;

public interface IPaymentGatewayService
{
    Task<PaymentResponse> WithdrawalViaPaymentGateway(WithdrawalBodyRequest data, short custDbGroupId);
}

public class PaymentGatewayService(
    ICommonService commonService,
    Fanex.Logging.ILogger logger,
    PaymentGatewayHttpClientHelper httpClient,
    PaymentGatewayApiConfig apiConfig) : IPaymentGatewayService
{
    private static PaymentResponse CreateErrorResponse(string message, int? code = null)
    {
        return new PaymentResponse
        {
            Code = code ?? -1,
            Message = message
        };
    }

    public async Task<PaymentResponse> WithdrawalViaPaymentGateway(WithdrawalBodyRequest data, short custDbGroupId)
    {
        var withdrawalApi = "/api/PaymentGateway/Withdrawal";
        if (!apiConfig.Enabled)
        {
            await logger.InfoAsync("WithdrawalViaPaymentGateway is Disabled now");
            return CreateErrorResponse("Payment gateway is not enabled.");
        }

        if (apiConfig.IsTestMode)
        {
            await logger.InfoAsync("WithdrawalViaPaymentGateway is Enabled and in Test Mode now");
            var testCode = Random.Shared.Next(0, 5);
            return CreateErrorResponse("Payment gateway is in test mode. No transactions will be processed.", testCode);
        }

        var response = new PaymentResponse { Code = 409 };
        var stopwatch = Stopwatch.StartNew();
        var requestBody = JsonConvert.SerializeObject(data);
        var criteria = new AddRequestLogCommand(
            data.AgentId,
            data.CustId,
            nameof(WithdrawalViaPaymentGateway),
            withdrawalApi,
            requestBody);
        try
        {
            response = await httpClient.PostAsync<WithdrawalBodyRequest, PaymentResponse>(withdrawalApi, data);

            criteria.StatusCode = response.Code;
            criteria.ResponseBody = JsonConvert.SerializeObject(response);
        }
        catch (Exception ex)
        {
            criteria.StatusCode = ex is HttpRequestException { StatusCode: not null } httpEx ? (int)httpEx.StatusCode.Value : 500;
            criteria.ResponseBody = ex.Message;

            await logger.ErrorAsync(ex.Message, ex);
        }
        finally
        {
            stopwatch.Stop();
            var duration = (int)stopwatch.Elapsed.TotalMilliseconds;
            criteria.Duration = duration;

            await commonService.WriteLogRequestResponse(criteria, custDbGroupId);
        }

        return response;
    }
}