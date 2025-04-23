namespace AlphaPlus.CashService.API.Models.Transactions;

using System;
using Enum;

public class TransactionModel
{
    public int TransactionId { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public bool IsDeposit { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public TransactionStatus Status { get; set; }
    public TransactionType TransactionType { get; set; }
    public string ReferenceNumber { get; set; }
    public DateTime DateTime { get; set; }
}