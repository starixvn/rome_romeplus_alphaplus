namespace AlphaPlus.Core.Criteria;

using Fanex.Data;
using Fanex.Data.Repository;

public abstract class SiteDbBaseCommand(int siteDbGroupId) : NonQueryCommand
{
    [SpParam(Ignored = true)]
    public int SiteDbGroupId { get; } = siteDbGroupId;
}

public abstract class SiteDbBaseCriteria(int siteDbGroupId) : CriteriaBase
{
    [SpParam(Ignored = true)]
    public int SiteDbGroupId { get; } = siteDbGroupId;
}