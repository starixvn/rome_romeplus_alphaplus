namespace AlphaPlus.Settings.Site.Features.CustomerList.Requests
{
    using AlphaPlus.Core.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using AlphaPlus.Settings.Site.Models;
    using MediatR;
    using System.Collections.Generic;

    public class UpdateMultipleDoubleCommCommand : CustomerStatusQueryBase, IRequest<IEnumerable<Settings.MemberInfo.Domain.Models.Result>>
    {
        private readonly IUser viewer;

        public UpdateMultipleDoubleCommCommand() : this(null){}

        public UpdateMultipleDoubleCommCommand(IUser viewer) : base(viewer)
        {
            this.viewer = viewer ?? new User(UserSession.Current);
        }

        public override bool IsValid()
        => !string.IsNullOrEmpty(SearchAccountIds) && CanUpdateAccount && !IsInternal && viewer.LevelId == SearchAccountRoleId + 1;

        public string SearchAccountIds { get; set; }

        public bool EnabledDoubleComm { get; set; }
    }
}