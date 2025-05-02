using System.ComponentModel.DataAnnotations;

namespace AlphaPlus.CashService.API.Requests.Payments;

public class SetDailyWithdrawalCountRequest
{
    [Required]
    public int CustId { get; set; }

    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Count must be a non-negative number")]
    public int Count { get; set; }
} 