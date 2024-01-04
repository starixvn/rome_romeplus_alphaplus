using AlphaPlus.BetsForecast.Domain.Shared;

namespace AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;

internal class GetMoneyLineCriteria(byte custLevel, int custId, string language) : GetTotalBetsCriteriaBase(custLevel, custId)
{
    public string Language { get; } = language;

    public override string GetSettingKey()
        => CustLevel switch
        {
            UserLevel.Super => "Age_TotalBets_Sel2021_SAg",
            UserLevel.Master => "Age_TotalBets_Sel2021_MAg",
            UserLevel.Agent => "Age_TotalBets_Sel2021_Ag",
            _ => throw new ArgumentException("Invalid enum value for CustLevel", nameof(CustLevel)),
        };
}