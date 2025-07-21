namespace AlphaPlus.CashService.API.Features.Shared.Services;

using System.Diagnostics;
using Criteria;
using Helpers;
using Models;
using Newtonsoft.Json;
using Requests;
using Requests.PaymentGateway;

public interface IPaymentGatewayService
{
    Task<PaymentResponse> TestConnection(TestConnectionRequest request, BaseActionRequest actionRequest);

    Task<PaymentResponse> WithdrawalViaPaymentGateway(WithdrawalBodyRequest data, short custDbGroupId);
}

public class PaymentGatewayService(
    ICommonService commonService,
    Fanex.Logging.ILogger logger,
    HttpClientHelper httpClient,
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
        var checkConfig = await CheckAPIConfig(nameof(WithdrawalViaPaymentGateway));
        if (checkConfig != null)
        {
            return checkConfig;
        }

        const string withdrawalApi = "/api/PaymentGateway/Withdrawal";
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
            response = await httpClient.PostAsync<WithdrawalBodyRequest, PaymentResponse>(apiConfig.BaseUrl, withdrawalApi, data);

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

    public async Task<PaymentResponse> TestConnection(TestConnectionRequest request, BaseActionRequest actionRequest)
    {
        var apiRequest = new
        {
            providerId = request.ProviderId,
            transactionType = request.TransactionType,
            paymentMethod = request.PaymentMethod,
            apiConfig = new
            {
                MerchantNo = request.APIConfig.MerchantNo,
                AppSecret = request.APIConfig.AppSecret,
                SecretKey = request.APIConfig.SecretKey
            }
        };

        return await PostAsync("/api/PaymentGateway/TestConnection", nameof(TestConnection), apiRequest, actionRequest);
    }

    public async Task<PaymentResponse> PostAsync(string apiURL, string functionName, object apiRequest, BaseActionRequest actor)
    {
        var checkConfig = await CheckAPIConfig(functionName);
        if (checkConfig != null)
        {
            return checkConfig;
        }

        var response = new PaymentResponse { Code = 409 };
        var stopwatch = Stopwatch.StartNew();

        var criteria = new AddRequestLogCommand(
            actor.UserId,
            actor.ReceivedCustId,
            functionName,
            apiConfig.BaseUrl.TrimEnd('/') + apiURL,
            JsonConvert.SerializeObject(apiRequest),
            subAccId: actor.SubAccId);

        try
        {
            response = await httpClient.PostAsync<object, PaymentResponse>(apiConfig.BaseUrl, apiURL, apiRequest);

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

            await commonService.WriteLogRequestResponse(criteria, actor.CustDBGroupId);
        }

        return response;
    }

    private async Task<PaymentResponse> CheckAPIConfig(string functionName)
    {
        if (!apiConfig.Enabled)
        {
            var message = $"Payment gateway is disabled now for {functionName}.";
            await logger.InfoAsync(message);
            return CreateErrorResponse(message);
        }

        if (apiConfig.IsTestMode)
        {
            var message = $"Payment gateway is in test mode for {functionName}.";
            await logger.InfoAsync(message);

            var testCode = GetCustomTestCode();
            return CreateErrorResponse(message, testCode);
        }

        return null;
    }

    private static int GetCustomTestCode()
    {
        var rand = Random.Shared.Next(0, 10);
        return rand switch
        {
            < 3 => 0,
            < 6 => Random.Shared.Next(1, 4),
            _ => 4
        };
    }
}