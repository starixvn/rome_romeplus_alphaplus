namespace CashService.API.DTOs.Payments
{
    public class DailyLimitDTO
    {
        public bool IsDeposit { get; set; }
        public decimal MinPer { get; set; }
        public decimal MaxPer { get; set; }
    }
}