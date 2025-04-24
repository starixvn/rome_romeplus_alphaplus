namespace AlphaPlus.Core.Criteria;

public class CheckBelongingCommand(int sessionCustId, int subAccId, long transId, int? custId = null)
    : CheckBelongingInDataWarehouseCommand(sessionCustId, transId, custId)
{
    public int SubAccountId { get; } = subAccId;

    public bool IsActive { get; set; } = true;

    public override string GetSettingKey() => "Age_CustomerSelCheckBelonging";

    public override bool IsValid() => SessionCustId > 0 && SubAccountId >= 0;
}
