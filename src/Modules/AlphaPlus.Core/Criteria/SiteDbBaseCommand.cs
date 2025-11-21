namespace AlphaPlus.Core.Criteria;

using Fanex.Data;
using Fanex.Data.Repository;

public abstract class SiteDbBaseCommand : NonQueryCommand
{
    [SpParam(Ignored = true)]
    public int SiteDbGroupId { get; set; }
}

public abstract class SiteDbBaseCriteria : CriteriaBase
{
    [SpParam(Ignored = true)]
    public int SiteDbGroupId { get; set; }
}