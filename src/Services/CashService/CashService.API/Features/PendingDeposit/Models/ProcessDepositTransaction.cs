namespace AlphaPlus.CashService.API.Features.PendingDeposit.Models;

public class ProcessDepositTransaction
{
    public long TransId { get; set; }
    public int CustId { get; set; }
    public int ActorId { get; set; }
    public int SubAccId { get; set; }
    public int StatusId { get; set; }
    public decimal NewAmount { get; set; }
    public short SiteDbGroupId { get; set; }
    public short CustDbGroupId { get; set; }
    public decimal? AgentGivenCredit { get; set; }
    public decimal? CreditMembersGivenCredit { get; set; }
    public string Ip { get; set; }
    public string Remark { get; set; }
}