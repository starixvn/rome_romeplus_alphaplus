using AlphaPlus.BetsForecast.Domain.Shared;
using Fanex.Data;
using Fanex.Data.Repository;

namespace AlphaPlus.BetsForecast.Infrastructure.TotalBets.Criteria;

internal abstract class GetTotalBetsCriteriaBase(byte custLevel, int custId) : CriteriaBase
{
    [SpParam(Ignored = true)]
    public UserLevel CustLevel { get; } = (UserLevel)custLevel;

    public int CustId { get; } = custId;

    public override bool IsValid() => CustId > 0;
}