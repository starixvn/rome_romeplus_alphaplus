namespace AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;

internal class GetHDPOULiveCriteria(byte custLevel, int custId) : GetTotalBetsCriteriaBase(custLevel, custId)
{
    public override string GetSettingKey()
        => CustLevel.IsSuperLevel()
        ? "Age_TotalBets_1378_Master"
        : (CustLevel.IsMasterLevel()
            ? "Age_TotalBets_1378_Agent"
            : "Age_TotalBets_1378_Member");
}
