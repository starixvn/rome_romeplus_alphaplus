namespace AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;

internal class GetMoneyLineCriteria(byte custLevel, int custId) : GetTotalBetsCriteriaBase(custLevel, custId)
{
    public override string GetSettingKey()
        => CustLevel.IsSuperLevel()
        ? "Age_TotalBets_Sel2021_SAg"
        : (CustLevel.IsMasterLevel()
            ? "Age_TotalBets_Sel2021_MAg"
            : "Age_TotalBets_Sel2021_Ag");
}