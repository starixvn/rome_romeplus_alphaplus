namespace AlphaPlus.CashService.Site.Features.Promotion.Models;

using AlphaPlus.CashService.Site.Helpers;
using AlphaPlus.Shared.AppResources.CashServices;
using Enum;
using Helpers;
using Transactions.Enum;

public class PromotionTransaction
{
    public int PromotionId { get; set; }
    public string TransId { get; set; }
    public int CustId { get; set; }
    public string Username { get; set; }
    public string Nickname { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public int StatusId { get; set; }
    public string Status => EnumHelper.GetEnumDescriptionByNumber<TransactionStatus>(StatusId, CashServicesResx.ResourceManager);
    public string Title { get; set; }
    public int PromotionType { get; set; }
    public string PromotionTypeName => EnumHelper.GetEnumDescriptionByNumber<PromotionType>(PromotionType, CashServicesResx.ResourceManager);
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}

public class PromotionTransactionResponse
{
    public IEnumerable<PromotionTransaction> PromotionTransactions { get; set; }
    public int TotalCount { get; set; }
}