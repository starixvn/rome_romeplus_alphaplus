using AlphaPlus.CashService.API.Features.Payments.Constants;

namespace AlphaPlus.CashService.API.Features.Payments.Models
{
    public class TransactionTypeModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int TransactionType { get; set; }
        public bool IsDeposit => TransactionType == PaymentTransactionTypeConstants.Deposit;
    }
} 