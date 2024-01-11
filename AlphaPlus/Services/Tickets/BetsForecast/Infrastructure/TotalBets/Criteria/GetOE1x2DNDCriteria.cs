using AlphaPlus.BetsForecast.Domain.Shared;

namespace AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;

internal class GetOE1X2DNDCriteria(byte custLevel, int custId, string language) : GetMoneyLineCriteria(custLevel, custId, language)
{
    public override string GetSettingKey()
        => CustLevel switch
        {
            UserLevel.Super => "Age_TotalBets_Sel25_SAg",
            UserLevel.Master => "Age_TotalBets_Sel25_MAg",
            UserLevel.Agent => "Age_TotalBets_Sel25_Ag",
            _ => throw new ArgumentException("Invalid enum value for CustLevel", nameof(CustLevel)),
        };
}