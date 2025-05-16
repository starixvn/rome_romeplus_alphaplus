namespace AlphaPlus.CashService.API.Criteria.Transactions;

using Fanex.Data.Repository;

public class GetAllTransactionsWithPagingCriteria : CriteriaBase
{
    public int TransactionType { get; set; }    

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }

    public string PaymentFrom { get; set; }

    public int PaymentStatus { get; set; }

    public int PageIndex { get; set; } = 1;

    public int PageSize { get; set; } = 100;

    public int TotalRecords { get; set; }

    public override string GetSettingKey() => "Age_Transaction_GetAll";

    public override bool IsValid() => true;
}