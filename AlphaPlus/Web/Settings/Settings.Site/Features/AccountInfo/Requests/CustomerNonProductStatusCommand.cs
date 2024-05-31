using AlphaPlus.Settings.Site.Features.Shared;
using AlphaPlus.Settings.Site.Features.Shared.Models;

namespace AlphaPlus.Settings.Site.Features.AccountInfo.Requests
{
    public class CustomerNonProductStatusCommand : BaseQuery
    {
        public int SearchAccountId { get; set; }

        public string ActorName => viewer.Name;

        public byte SearchAccountRoleId { get; set; }

        public bool StatusValue { get; set; }

        public string StatusName { get; set; }

        public int ActorId => viewer.Id;

        public int SubAccountId => viewer.SubAccId;

        public int ServerId { get; }

        public override bool IsValid()
        {
            var isValid = base.IsValid() && !string.IsNullOrWhiteSpace(StatusName) && CanUpdateAccount && !IsInternal;
            return !StatusName.Equals(StatusNames.DoubleComm) ? isValid : isValid && IsValidDoubleComm;
        }

        internal bool IsValidDoubleComm => viewer.LevelId == SearchAccountRoleId + 1;
    }
}