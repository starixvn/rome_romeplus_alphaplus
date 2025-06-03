using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Transactions.Criteria;

public class GetAccountCriteria : CriteriaBase
{
    public override string GetSettingKey() => "Age_Transaction_GetAccount";

    public override bool IsValid() => true;
}