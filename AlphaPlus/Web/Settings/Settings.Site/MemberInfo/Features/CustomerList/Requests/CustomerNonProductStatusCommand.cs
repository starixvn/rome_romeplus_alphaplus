namespace AlphaPlus.Settings.Site.Features.CustomerList.Requests
{
    using AlphaPlus.Core.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.Shared.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using AlphaPlus.Settings.Site.Models;
    using MediatR;

    public class CustomerNonProductStatusCommand : CustomerStatusQueryBase, IRequest<Settings.MemberInfo.Domain.Models.Result>
    {
        public CustomerNonProductStatusCommand() : this(null){}
        public CustomerNonProductStatusCommand(IUser viewer) : base(viewer)
        {
        }


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