using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Shared.Models;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.CustomerList.Requests
{
    public class CustomerNonProductStatusCommand : CustomerStatusQueryBase, IRequest<Result>
    {
        public bool StatusValue { get; set; }

        public string StatusName { get; set; }

        public override bool IsValid()
        {
            var isValidSearchAccount = SearchAccountId > 0 && !SearchAccountId.Equals(viewer.Id);
            var allowUpdate = CanUpdateAccount && !IsInternal;

            var isValid = isValidSearchAccount && IsValidName(StatusName) && allowUpdate;
            return !StatusName.Equals(StatusNames.DoubleComm) ? isValid : isValid && IsValidDoubleComm;
        }

        protected bool IsValidDoubleComm => viewer.LevelId == SearchAccountRoleId + 1;
    }
}