using System.ComponentModel;

namespace AlphaPlus.CashService.API.Enum;

public enum TransactionStatus
{
    Completed,
    Rejected,

    [Description("Provider Rejected")]
    ProviderRejected,

    Expired
}