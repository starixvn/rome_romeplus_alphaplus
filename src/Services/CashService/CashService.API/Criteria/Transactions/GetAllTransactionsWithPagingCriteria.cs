namespace AlphaPlus.CashService.API.Criteria.Transactions;

using Fanex.Data;
using Fanex.Data.Repository;

public class GetAllTransactionsWithPagingCriteria : CriteriaBase
{
    public int CustId { get; set; }

    public int TransactionType { get; set; }

    [SpParam(AllowNull = true)]
    public DateTime? FromDate { get; set; }

    [SpParam(AllowNull = true)]
    public DateTime? ToDate { get; set; }

    public string PaymentFrom { get; set; }

    public int PaymentStatus { get; set; }

    public int PageIndex { get; set; } = 1;

    public int PageSize { get; set; } = 100;

    public int TotalRecords { get; set; }

    public override string GetSettingKey() => "Age_Transaction_GetAll";

    public override bool IsValid() => true;
}