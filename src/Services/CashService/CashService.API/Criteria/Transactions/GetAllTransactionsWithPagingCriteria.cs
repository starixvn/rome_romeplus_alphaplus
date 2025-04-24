namespace AlphaPlus.CashService.API.Criteria.Transactions;

using Fanex.Data.Repository;

public class GetAllTransactionsWithPagingCriteria : CriteriaBase
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int FromAccount { get; set; }
    public int ToAccount { get; set; }
    public string? Status { get; set; }
    public string TransactionType { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 1000;
    public int TotalItems { get; set; }

    public override string GetSettingKey() => "Age_Transaction_GetAll";

    public override bool IsValid() => true;
}