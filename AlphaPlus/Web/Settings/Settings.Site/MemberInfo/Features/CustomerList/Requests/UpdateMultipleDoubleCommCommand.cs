namespace Alpha.MemberInfo.Site.Features.CustomerList.Requests
{
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;
    using System.Collections.Generic;

    public class UpdateMultipleDoubleCommCommand : CustomerStatusQueryBase, IRequest<IEnumerable<Result>>
    {
        private readonly IUser viewer;

        public UpdateMultipleDoubleCommCommand(IUser viewer) : base(viewer)
        {
            this.viewer = viewer;
        }

        public override bool IsValid()
        => !string.IsNullOrEmpty(SearchAccountIds) && CanUpdateAccount && !IsInternal && viewer.LevelId == SearchAccountRoleId + 1;

        public string SearchAccountIds { get; set; }

        public bool EnabledDoubleComm { get; set; }
    }
}