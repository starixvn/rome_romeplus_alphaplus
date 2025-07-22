using AgencyCash.API.Shared.DataAnnotations;

namespace AgencyCash.API.Payments.Requests;

public class UpdateTransactionStatusRequest
{
    [PositiveNumber]
    public int AgentId { get; init; }

    [PositiveNumber]
    public int CustId { get; init; }

    [PositiveNumber]
    public byte StatusId { get; init; }

    public string ProviderTransId { get; init; }

    public string IP { get; init; }
}