namespace AlphaPlus.CashService.API.Features.Shared.Models;

public class WriteLogDBCommandModel
{
    public int LogTypeId { get; set; }
    public int UserId { get; set; }
    public int CustId { get; set; }
    public int? ActionId { get; set; }
    public string TrackingValue { get; set; }
    public string TrackingValue2 { get; set; }
    public string TrackingValueMask { get; set; }
    public string TrackingValueMask2 { get; set; }
    public int FromCustId { get; set; }
    public int ToCustId { get; set; }
    public string Ip { get; set; }
    public string Doing { get; set; }

    public string Duration { get; set; }
}