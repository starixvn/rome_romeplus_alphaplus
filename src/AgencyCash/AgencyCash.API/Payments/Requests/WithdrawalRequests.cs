namespace AgencyCash.API.Payments.Requests;

public class CreateManualWithdrawalRequest
{
    public int CustId { get; set; }
    public int AgentId { get; set; }
    public string PaymentFrom { get; set; }
    public string PaymentTo { get; set; }
    public decimal Amount { get; set; }
    public string IP { get; set; }
}

public class CreatePayoutRequest
{
    public int CustId { get; set; }
    public int AgentId { get; set; }
    public string PaymentFrom { get; set; }
    public string PaymentTo { get; set; }
    public decimal Amount { get; set; }
    public string IP { get; set; }
    public string ReferenceId { get; set; }
    public string Description { get; set; }
    public bool IsAutoApprove { get; set; }
} 