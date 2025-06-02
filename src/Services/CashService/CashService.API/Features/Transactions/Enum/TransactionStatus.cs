using System.ComponentModel;

namespace AlphaPlus.CashService.API.Features.Transactions.Enum;

public enum TransactionStatus
{
    Completed,
    Rejected,

    [Description("Provider Rejected")]
    ProviderRejected,

    Expired
}