using AlphaPlus.CashService.Site.Features.Shared.Requests;
using AlphaPlus.Core.Models;

namespace AlphaPlus.CashService.Site.Features.Payments.Requests;

public class SetTransactionLimitRequest : BaseActionRequest
{
    public int DailyWithdrawalCount { get; set; }
    public decimal MinPerDepositTransaction  { get; set; }
    public decimal MaxPerDepositTransaction  { get; set; }
    public decimal MinPerWithdrawalTransaction  { get; set; }
    public decimal MaxPerWithdrawalTransaction  { get; set; }
} 