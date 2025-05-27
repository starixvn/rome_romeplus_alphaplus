namespace AlphaPlus.PaymentService.API.Payments.Models;

public class LogEntry
{
    public int LogTypeId { get; init; }
    public int UserId { get; init; }
    public int CustId { get; init; }
    public int ActionId { get; init; }
    public string TrackingValue { get; init; }
    public string TrackingValue2 { get; init; }
    public int FromCustId { get; init; }
    public int ToCustId { get; init; }
    public string IP { get; init; }
    public string Doing { get; init; }
}