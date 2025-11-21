namespace AlphaPlus.Core.Criteria;

using Fanex.Data;
using Fanex.Data.Repository;

public abstract class SpSuffixBaseCriteria(string suffix) : CriteriaBase
{
    [SpParam(Ignored = true)]
    public string Suffix { get; set; } = suffix;
}