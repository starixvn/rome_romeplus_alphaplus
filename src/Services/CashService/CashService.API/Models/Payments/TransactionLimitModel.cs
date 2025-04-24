using System;

namespace AlphaPlus.CashService.API.Models.Payments
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