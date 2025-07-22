namespace AlphaPlus.CashService.Site.Features.Transactions.Models;

using Shared.Requests;

public class ProcessWithdrawalTransaction : BaseActionRequest
{
    public long TransId { get; set; }
    public int CustId { get; set; }
    public string Remark { get; set; }
    public int PaymentMethodId { get; set; }
    public int ProviderId { get; set; }
    public string PaymentFrom { get; set; }
    public IFormFile Image { get; set; }
    public byte[] Payslip { get; set; }
    public bool IsRemovePayslip { get; set; }
}