namespace AlphaPlus.CashService.API.Features.Transactions.Models;

using System.Numerics;
using System.Text.Json.Serialization;

public class TransactionModel
{
    [JsonIgnore]
    public BigInteger TransId { get; set; }

    [JsonPropertyName("transId")]
    public string TransIdString => TransId.ToString();

    public int CustId { get; set; }
    public string Username { get; set; }
    public string Nickname { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string PaymentFrom { get; set; }
    public string PaymentTo { get; set; }
    public int StatusId { get; set; }
    public decimal BetCredit { get; set; }
    public int TransactionTypeId { get; set; }
    public string TransactionType { get; set; }
    public string Remark { get; set; }
    public byte[] Payslip { get; set; }
    public bool IsAllowRevert { get; set; }
    public string OperatedBy { get; set; }
    public string Base64Payslip => Payslip is null ? string.Empty : Convert.ToBase64String(Payslip);
    public DateTime CreatedTime { get; set; }
    public DateTime? LastUpdatedDate { get; set; }
    public DateTime? OperatedTime { get; set; }
}