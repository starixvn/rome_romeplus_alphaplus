namespace AlphaPlus.CashService.Site.Features.Transactions.Enum;

using System.ComponentModel;
using AlphaPlus.Shared.AppResources.CashServices;

public enum TransactionStatus
{
    [Description(nameof(CashServicesResx.TransactionStatus_Expired))]
    Pending = 2,

    [Description(nameof(CashServicesResx.TransactionStatus_Expired))]
    Expired = 3,

    [Description(nameof(CashServicesResx.TransactionStatus_Completed))]
    Completed = 4,

    [Description(nameof(CashServicesResx.TransactionStatus_Rejected))]
    Rejected = 5,

    [Description(nameof(CashServicesResx.TransactionStatus_ProviderRejected))]
    ProviderRejected = 6,
}