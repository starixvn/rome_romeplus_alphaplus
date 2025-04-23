namespace AlphaPlus.CashService.API.Criteria.Transactions;

using Enum;
using Fanex.Data.Repository;

public class GetAllTransactionsWithPagingCriteria : CriteriaBase
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int FromAccount { get; set; }
    public int ToAccount { get; set; }
    public TransactionStatus? Status { get; set; }
    public TransactionType TransactionType { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalItems { get; set; }

    public override string GetSettingKey() => "Age_Transaction_GetAll";

    public override bool IsValid() => true;
}