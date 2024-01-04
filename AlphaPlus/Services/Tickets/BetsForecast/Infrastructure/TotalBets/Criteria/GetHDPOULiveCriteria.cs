
using AlphaPlus.BetsForecast.Domain.Shared;

namespace AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;

internal class GetHDPOULiveCriteria(byte custLevel, int custId) : GetTotalBetsCriteriaBase(custLevel, custId)
{
    public override string GetSettingKey()
        => CustLevel switch
        {
            UserLevel.Super => "Age_TotalBets_1378_Master",
            UserLevel.Master => "Age_TotalBets_1378_Agent",
            UserLevel.Agent => "Age_TotalBets_1378_Member",
            _ => throw new ArgumentException("Invalid enum value for CustLevel", nameof(CustLevel)),
        };
}
