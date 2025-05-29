namespace AgencyCash.API.Payments.Requests;

public class ManualWithdrawalRequest
{
    public int CustId { get; set; }
    public int AgentId { get; set; }
    public string PaymentFrom { get; set; }
    public string PaymentTo { get; set; }
    public decimal Amount { get; set; }
    public string IP { get; set; }
}