using AlphaPlus.CashService.API.Features.Shared.Request;

namespace AlphaPlus.CashService.API.Features.Payments.Requests;

public class SetDailyWithdrawalCountRequest : BaseActionRequest
{
    public int Count { get; set; }
} 