using AlphaPlus.CashService.API.Features.Shared.Requests;

namespace AlphaPlus.CashService.API.Features.Payments.Requests;

public class SetTransactionLimitRequest : BaseActionRequest
{
    public int DailyWithdrawalCount { get; set; }
    public decimal MinPerDepositTransaction  { get; set; }
    public decimal MaxPerDepositTransaction  { get; set; }
    public decimal MinPerWithdrawalTransaction  { get; set; }
    public decimal MaxPerWithdrawalTransaction  { get; set; }
} 