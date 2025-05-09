namespace AlphaPlus.CashService.API.Models.PendingCommons;

public class CreateManualModel
{
    public int CustId { get; set; }

    public int UplineId { get; set; }

    public int SubAccId { get; set; }

    public decimal AgentGivenCredit { get; set; }

    public decimal MemberUsedCredit { get; set; }

    public decimal MinLimitAmt { get; set; }

    public decimal MaxLimitAmt { get; set; }

    public int WithdrawalLimitCnt { get; set; }

    public string PaymentFrom { get; set; }

    public string PaymentTo { get; set; }

    public decimal Amount { get; set; }

    public byte[] PaySlip { get; set; }

    public string Ip { get; set; }

    public string Remark { get; set; }

    public int ProviderId { get; set; }
}