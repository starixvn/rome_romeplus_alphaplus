namespace AlphaPlus.CashService.Site.Features.Transactions.Enum;

using System.ComponentModel;
using AlphaPlus.Shared.AppResources.CashServices;

public enum TransactionType
{
    [Description(nameof(CashServicesResx.TransactionType_ManualDeposit))]
    ManualDeposit = 1,

    [Description(nameof(CashServicesResx.TransactionType_AutoDeposit))]
    AutoDeposit = 2,

    [Description(nameof(CashServicesResx.TransactionType_ManualWithdrawal))]
    ManualWithdrawal = 3,

    [Description(nameof(CashServicesResx.TransactionType_AutoWithdrawal))]
    AutoWithdrawal = 4,

    [Description(nameof(CashServicesResx.TransactionType_SystemAdjust))]
    SystemAdjust = 10
}