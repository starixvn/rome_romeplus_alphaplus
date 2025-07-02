using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Shared.Criteria;

public class GetSetupCheckListCriteria(int custId) : CriteriaBase
{
    public int CustId { get; } = custId;

    public override string GetSettingKey() => "Age_CashService_ConfigureSignUpChecklist";

    public override bool IsValid() => true;
} 