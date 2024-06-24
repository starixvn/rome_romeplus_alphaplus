using AlphaPlus.Settings.Site.Features.Shared;

namespace AlphaPlus.Settings.Site.Features.AccountInfo.Requests
{
    public class UpdateGivenCreditCommand : BaseQuery
    {
        public virtual int SearchAccountId { get; set; }

        public byte SearchAccountRoleId { get; set; }

        public decimal CreditAmount { get; set; }

        public int ActorId => viewer.Id;

        public int SubAccountId => viewer.SubAccId;

        public override bool IsValid() =>
            base.IsValid() &&
            CanUpdateAccount &&
            !IsInternal &&
            SearchAccountRoleId < viewer.LevelId;
    }
}