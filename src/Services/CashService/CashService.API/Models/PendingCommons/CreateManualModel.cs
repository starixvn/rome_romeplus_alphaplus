namespace AlphaPlus.CashService.API.Models.PendingCommons;

public class CreateManualModel
{
    public int CustId { get; set; }

    public int UplineId { get; set; }

    public int SubAccId { get; set; }

    public string PaymentFrom { get; set; }

    public string PaymentTo { get; set; }

    public decimal Amount { get; set; }

    public byte[] Payslip { get; set; }

    public string Ip { get; set; }

    public string Remark { get; set; }

    public int CustDbGroupId { get; set; }
}