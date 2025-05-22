using AlphaPlus.CashService.Site.Features.Shared.Requests;

namespace AlphaPlus.CashService.Site.Features.Payments.Requests
{
    public class SetDailyWithdrawalCountRequest : BaseActionRequest
    {
        public int Count { get; set; }
    }
} 