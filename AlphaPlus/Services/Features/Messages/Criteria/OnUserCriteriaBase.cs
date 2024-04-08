using Fanex.Data.Repository;
using AlphaPlus.Features.Messages.API.Configurations;

namespace AlphaPlus.Features.Messages.API.Criteria
{
    public abstract class OnUserCriteriaBase : CriteriaBase
    {
        public override bool IsValid()
        {
            return !AppSettings.IsDisabledRecheckin;
        }
    }
}