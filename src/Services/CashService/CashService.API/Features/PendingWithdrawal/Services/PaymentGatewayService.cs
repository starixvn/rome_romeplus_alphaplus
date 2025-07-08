namespace AlphaPlus.CashService.API.Features.PendingWithdrawal.Services;

using AlphaPlus.CashService.API.Features.Shared.Models;
using Helpers;
using Models.PaymentGateway;

public interface IPaymentGatewayService
{
    Task<PaymentResponse<WithdrawalDto>> WithdrawalViaPaymentGateway(WithdrawalBodyRequest data);
}

public class PaymentGatewayService(PaymentGatewayHttpClientHelper httpClient) : IPaymentGatewayService
{
    public async Task<PaymentResponse<WithdrawalDto>> WithdrawalViaPaymentGateway(WithdrawalBodyRequest data)
        => await httpClient.PostAsync<WithdrawalBodyRequest, PaymentResponse<WithdrawalDto>>("/api/PaymentGateway/Withdrawal", data);
}