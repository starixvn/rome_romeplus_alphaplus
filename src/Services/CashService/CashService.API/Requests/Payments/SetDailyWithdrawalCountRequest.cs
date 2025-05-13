namespace AlphaPlus.CashService.API.Requests.Payments;

public class SetDailyWithdrawalCountRequest : BaseActionRequest
{
    public int Count { get; set; }
} 