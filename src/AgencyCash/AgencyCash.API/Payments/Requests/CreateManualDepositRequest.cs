namespace AgencyCash.API.Payments.Requests;

public class CreateManualDepositRequest
{
    public int CustId { get; init; }

    public int AgentId { get; init; }

    public string PaymentFrom { get; init; }

    public string PaymentTo { get; init; }

    public decimal Amount { get; init; }

    public byte[] PaySlip { get; init; }

    public string IP { get; init; }
}