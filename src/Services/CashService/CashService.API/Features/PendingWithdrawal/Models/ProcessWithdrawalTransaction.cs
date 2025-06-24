namespace AlphaPlus.CashService.API.Features.PendingWithdrawal.Models;

using Shared.Requests;

public class ProcessWithdrawalTransaction : BaseActionRequest
{
    public long TransId { get; set; }
    public int CustId { get; set; }
    public string PaymentFrom { get; set; }
    public int ProviderId { get; set; }
    public int PaymentMethodId { get; set; }
    public string Remark { get; set; }
    public int StatusId { get; set; }
    public byte[] Payslip { get; set; }
}