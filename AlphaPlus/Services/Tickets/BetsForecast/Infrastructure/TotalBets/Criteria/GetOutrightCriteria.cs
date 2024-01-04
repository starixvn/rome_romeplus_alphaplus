using AlphaPlus.BetsForecast.Domain.Shared;

namespace AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;

internal class GetOutrightCriteria(byte custLevel, int custId, string language) : GetMoneyLineCriteria(custLevel, custId, language)
{
    public override string GetSettingKey()
        => CustLevel switch
        {
            UserLevel.Super => "Age_TotalBets_Sel10_SAg",
            UserLevel.Master => "Age_TotalBets_Sel10_MAg",
            UserLevel.Agent => "Age_TotalBets_Sel10_Ag",
            _ => throw new ArgumentException("Invalid enum value for CustLevel", nameof(CustLevel)),
        };
}