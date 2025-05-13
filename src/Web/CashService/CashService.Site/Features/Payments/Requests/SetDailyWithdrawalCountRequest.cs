using AlphaPlus.CashService.Site.Features.Shared.Requests;
using AlphaPlus.Core.Models;

namespace AlphaPlus.CashService.Site.Features.Payments.Requests
{
    public class SetDailyWithdrawalCountRequest : BaseActionRequest
    {
        public int Count { get; set; }
    }
} 