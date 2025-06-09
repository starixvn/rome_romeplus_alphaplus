using AgencyCash.API.Shared.DataAnnotations;

namespace AgencyCash.API.Payments.Requests;

public class GetWithdrawalLimitRequest
{
    [PositiveNumber]
    public int AgentId { get; set; }

    [PositiveNumber]
    public int CustId { get; set; }
}