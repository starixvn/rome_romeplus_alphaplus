using System.ComponentModel;

namespace AlphaPlus.CashService.API.Features.Transactions.Enum;

public enum TransactionStatus
{
    Completed = 4,
    Rejected,

    [Description("Provider Rejected")]
    ProviderRejected,

    Expired
}