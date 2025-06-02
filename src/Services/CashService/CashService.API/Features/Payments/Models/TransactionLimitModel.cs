namespace AlphaPlus.CashService.API.Features.Payments.Models
{
    public class TransactionLimitModel
    {
        public int DailyWithdrawalCount { get; set; }
        public decimal MinPerDepositTransaction { get; set; }
        public decimal MaxPerDepositTransaction { get; set; }
        public decimal MinPerWithdrawalTransaction { get; set; }
        public decimal MaxPerWithdrawalTransaction { get; set; }
    }
}