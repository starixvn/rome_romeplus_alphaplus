namespace AlphaPlus.CashService.API.Features.Promotion.Models;

using Shared.Models;

public class PromotionTransaction : BaseTransaction
{
    public int PromotionId { get; set; }
    public int CustId { get; set; }
    public string Username { get; set; }
    public string Nickname { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public int StatusId { get; set; }
    public string Title { get; set; }
    public int PromotionType { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}