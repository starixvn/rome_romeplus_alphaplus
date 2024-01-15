namespace AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;

internal class GetNumberGameCriteria(byte custLevel, int custId) : GetTotalBetsCriteriaBase(custLevel, custId)
{
    public override string GetSettingKey()
        => CustLevel.IsSuperLevel()
        ? "Age_TotalBets_Sel161_SAg"
        : (CustLevel.IsMasterLevel()
            ? "Age_TotalBets_Sel161_MAg"
            : "Age_TotalBets_Sel161_Ag");
}