namespace AlphaPlus.CashService.API.Features.Payments.DTOs
{
    public class DailyLimitDTO
    {
        public int TransType { get; set; }

        public bool IsDeposit => TransType == 0;
        public decimal MinPer { get; set; }
        public decimal MaxPer { get; set; }
    }
}