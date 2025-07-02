using Fanex.Data.Repository;

namespace AlphaPlus.CashService.API.Features.Shared.Criteria;

public class GetSetupCheckListCriteria(int custId) : NonQueryCommand
{
    public int CustId { get; } = custId;

    public bool CompleteDepositMethods { get; set; } = false;
    public bool CompleteWithdrawalMethods { get; set; } = false;
    public bool CompleteLineID { get; set; } = false;

    public override string GetSettingKey() => "Age_CashService_SetupCheckList";

    public override bool IsValid() => true;
} 