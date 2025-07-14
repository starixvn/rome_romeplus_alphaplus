namespace AlphaPlus.CashService.API.Features.PendingWithdrawal.Models.PaymentGateway;

public class WithdrawalBodyRequest
{
    public int AgentId { get; set; }
    public int CustId { get; set; }
    public long TransId { get; set; }
    public decimal Amount { get; set; }
    public int ProviderId { get; set; }
    public WithdrawalMemberInfo MemberInfo { get; set; }
}

public class WithdrawalMemberInfo
{
    public string Name { get; set; }
    public string BankCode { get; set; }
    public string AccountNumber { get; set; }
}