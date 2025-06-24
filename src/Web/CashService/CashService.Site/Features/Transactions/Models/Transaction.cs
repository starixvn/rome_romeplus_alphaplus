namespace AlphaPlus.CashService.Site.Features.Transactions.Models;

using AlphaPlus.Shared.AppResources.CashServices;
using Enum;
using Helpers;

public record TransactionResponse
{
    public IEnumerable<Transaction> Items { get; set; }
}
public record Transaction
{
    public string TransId { get; set; }
    public int CustId { get; set; }
    public string Username { get; set; }
    public string Nickname { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal Amount { get; set; }
    public string PaymentFrom { get; set; }
    public string PaymentTo { get; set; }
    public decimal BetCredit { get; set; }
    public string Remark { get; set; }
    public int StatusId { get; set; }
    public string Status => EnumHelper.GetEnumDescriptionByNumber<TransactionStatus>(StatusId, CashServicesResx.ResourceManager);
    public int TransactionTypeId { get; set; }
    public string TransactionType => EnumHelper.GetEnumDescriptionByNumber<TransactionType>(TransactionTypeId, CashServicesResx.ResourceManager);
    public DateTime CreatedTime { get; set; }
    public bool IsAllowRevert { get; set; }
    public bool IsHadPayslip { get; set; }
    public string CreatedBy { get; set; }
    public string Base64Payslip { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
    public DateTime? OperatedTime { get; set; }
}