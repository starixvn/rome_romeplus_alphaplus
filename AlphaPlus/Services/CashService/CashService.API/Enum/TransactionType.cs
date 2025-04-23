namespace AlphaPlus.CashService.API.Enum;

using System.ComponentModel;

public enum TransactionType
{
    [Description("Auto Withdrawal")]
    AutoWithdrawal,

    [Description("Manual Withdrawal")]
    ManualWithdrawal,

    [Description("Manual Deposit")]
    ManualDeposit,

    [Description("Auto Deposit")]
    AutoDeposit
}