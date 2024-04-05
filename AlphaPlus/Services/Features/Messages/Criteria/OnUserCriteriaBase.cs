using Fanex.Data.Repository;
using AlphaPlus.Features.Messages.Configurations;

namespace AlphaPlus.Features.Messages.Criteria
{
    public abstract class OnUserCriteriaBase : CriteriaBase
    {
        public override bool IsValid()
        {
            return !AppSettings.IsDisabledRecheckin;
        }
    }
}