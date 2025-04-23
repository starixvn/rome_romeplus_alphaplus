namespace AlphaPlus.Settings.MemberInfo.Domain.Features.Shared.Models;

public class CustInfo(int custId, string custName, byte custLevelId)
{
    public int CustId { get; init; } = custId;

    public string CustName { get; init; } = custName;

    public byte CustLevelId { get; init; } = custLevelId;
}