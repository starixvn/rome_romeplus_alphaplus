using System.ComponentModel.DataAnnotations;

namespace AlphaPlus.CashService.Site.Features.Payments.Requests
{
    public class SetDailyWithdrawalCountRequest
    {
        [Required]
        public int CustId { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Daily withdrawal count must be a non-negative number")]
        public int Count { get; set; }
    }
} 