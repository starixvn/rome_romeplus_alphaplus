using AlphaPlus.BetsForecast.Domain.Shared;

namespace AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;

internal class GetFirstGoalLastGoalCriteria(byte custLevel, int custId, string language) : GetMoneyLineCriteria(custLevel, custId, language)
{
    public override string GetSettingKey()
        => CustLevel switch
        {
            UserLevel.Super => "Age_TotalBets_Sel14_SAg",
            UserLevel.Master => "Age_TotalBets_Sel14_MAg",
            UserLevel.Agent => "Age_TotalBets_Sel14_Ag",
            _ => throw new ArgumentException("Invalid enum value for CustLevel", nameof(CustLevel)),
        };
}
