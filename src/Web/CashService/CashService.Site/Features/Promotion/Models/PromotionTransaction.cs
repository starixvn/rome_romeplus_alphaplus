namespace AlphaPlus.CashService.Site.Features.Promotion.Models;

public class PromotionTransaction
{
    public int PromotionId { get; set; }
    public long TransId { get; set; }
    public int CustId { get; set; }
    public string Username { get; set; }
    public string Nickname { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public int StatusId { get; set; }
    public string Status { get; set; }
    public string Title { get; set; }
    public int PromotionType { get; set; }
    public string PromotionTypeName { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}

public class PromotionTransactionResponse
{
    public IEnumerable<PromotionTransaction> PromotionTransactions { get; set; }
    public int MyProperty { get; set; }
}