using AlphaPlus.CashService.API.Constants;

namespace AlphaPlus.CashService.API.Models.Payments
{
    public class TransactionTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TransactionType { get; set; }
        public bool IsDeposit => TransactionType == PaymentTransactionTypeConstants.Deposit;
    }
} 