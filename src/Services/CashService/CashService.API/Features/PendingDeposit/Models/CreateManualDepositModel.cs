namespace AlphaPlus.CashService.API.Features.PendingDeposit.Models;

public class CreateManualDepositModel
{
    public int CustId { get; set; }

    public int UplineId { get; set; }

    public int SubAccId { get; set; }

    public string PaymentFrom { get; set; }

    public string PaymentTo { get; set; }

    public decimal Amount { get; set; }

    public byte[] PaySlip { get; set; }

    public string Ip { get; set; }

    public string Remark { get; set; }

    public short CustDbGroupId { get; set; }

    public short SiteDbGroupId { get; set; }
}