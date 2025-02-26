using Fanex.Data.Repository;

namespace AlphaPlus.Core.Criteria;

public class CheckBelongingInDataWarehouseCommand(int sessionCustId, long transId, int? custId = null) : NonQueryCommand
{
    public int SessionCustId { get; } = sessionCustId;

    public long TransId { get; } = transId;

    public int? CustId { get; } = custId;

    public byte RoleId { get; set; } = 0;

    public override string GetSettingKey() => "Age_CustomerSelCheckBelonging_OLD";

    public override bool IsValid() => SessionCustId > 0;
}
