using AlphaPlus.BetsForecast.Domain.Shared;

namespace AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;

internal class GetHalfTimeFullTimeCriteria(byte custLevel, int custId, string language) : GetMoneyLineCriteria(custLevel, custId, language)
{
    public override string GetSettingKey()
        => CustLevel switch
        {
            UserLevel.Super => "Age_TotalBets_Sel16_SAg",
            UserLevel.Master => "Age_TotalBets_Sel16_MAg",
            UserLevel.Agent => "Age_TotalBets_Sel16_Ag",
            _ => throw new ArgumentException("Invalid enum value for CustLevel", nameof(CustLevel)),
        };
}