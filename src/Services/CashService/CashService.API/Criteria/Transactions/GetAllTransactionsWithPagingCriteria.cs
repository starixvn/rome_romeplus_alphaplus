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

    public static int PageIndex => 1;

    public static int PageSize =>  5000;

    public int TotalRecords { get; set; }

    public override string GetSettingKey() => "Age_Transaction_GetAll";

    public override bool IsValid() => true;
}