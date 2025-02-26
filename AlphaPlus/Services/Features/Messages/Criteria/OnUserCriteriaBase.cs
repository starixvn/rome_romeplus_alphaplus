using Fanex.Data.Repository;

namespace AlphaPlus.Features.Messages.API.Criteria
{
    public abstract class OnUserCriteriaBase : CriteriaBase
    {
        private readonly bool isDisabledRecheckin;

        public OnUserCriteriaBase(bool isDisabledRecheckin)
        {
            this.isDisabledRecheckin = isDisabledRecheckin;
        }

        public override bool IsValid()
        {
            return !isDisabledRecheckin;
        }
    }
}