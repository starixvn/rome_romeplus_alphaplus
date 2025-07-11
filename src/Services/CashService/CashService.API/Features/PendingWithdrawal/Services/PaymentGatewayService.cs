namespace AlphaPlus.CashService.API.Features.PendingWithdrawal.Services;

using AlphaPlus.CashService.API.Features.PendingWithdrawal.Criteria;
using AlphaPlus.CashService.API.Features.Shared.Models;
using Fanex.Data.Repository;
using Helpers;
using Models.PaymentGateway;

public interface IPaymentGatewayService
{
    Task<PaymentResponse<WithdrawalDto>> WithdrawalViaPaymentGateway(WithdrawalBodyRequest data);

    Task<AgentApiConfig> GetAgentPaymentGatewayInfo(int agentId, int transactionType, int providerId, int paymentMethodId);
}

public class PaymentGatewayService(IDynamicRepository dynamicRepository,
    PaymentGatewayHttpClientHelper httpClient,
    PaymentGatewayApiConfig apiConfig) : IPaymentGatewayService
{
    private static PaymentResponse<T> CreateErrorResponse<T>(string message, int? code = null, T data = null) where T : class
    {
        return new PaymentResponse<T>
        {
            Code = code ?? -1,
            Message = message,
            Data = data
        };
    }

    public async Task<PaymentResponse<WithdrawalDto>> WithdrawalViaPaymentGateway(WithdrawalBodyRequest data)
    {
        if (!apiConfig.Enabled)
        {
            return CreateErrorResponse<WithdrawalDto>("Payment gateway is not enabled.");
        }

        if (apiConfig.IsTestMode)
        {
            var testCode = Random.Shared.Next(0, 5);
            var testData = new WithdrawalDto
            {
                TransId = data.TransId,
                ProviderTransId = $"TEST Provider TransId-{data.TransId}"
            };
            return CreateErrorResponse("Payment gateway is in test mode. No transactions will be processed.", testCode, testCode == 0 ? testData : null);
        }

        ValidateInputParams(data);
        return await httpClient.PostAsync<WithdrawalBodyRequest, PaymentResponse<WithdrawalDto>>("/api/PaymentGateway/Withdrawal", data);
    }

    public async Task<AgentApiConfig> GetAgentPaymentGatewayInfo(int agentId, int transactionType,
        int providerId, int paymentMethodId)
        => await dynamicRepository.GetAsync<AgentApiConfig>(new GetAgentPaymentGatewayInfoCriteria
        {
            AgentId = agentId,
            PaymentMethodId = paymentMethodId,
            ProviderId = providerId,
            TransactionType = transactionType
        });

    private static void ValidateInputParams(BasePaymentGatewayRequest baseRequest)
    {
        if (baseRequest.ApiConfig is null || string.IsNullOrWhiteSpace(baseRequest.ApiConfig.AppSecret) ||
            string.IsNullOrWhiteSpace(baseRequest.ApiConfig.MerchantNo) ||
            string.IsNullOrWhiteSpace(baseRequest.ApiConfig.SecretKey))
        {
            throw new ArgumentException("Invalid API configuration. Please ensure that AppSecret, MerchantNo, and SecretKey are provided.");
        }
    }
}