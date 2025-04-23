namespace AlphaPlus.CashService.API.Criteria.Transactions;

using Fanex.Data.Repository;

public class GetTransactionsByIdCriteria : CriteriaBase
{
    public int TransactionId { get; set; }

    public override string GetSettingKey() => "Age_Transaction_GetById";

    public override bool IsValid() => TransactionId > 0;
}