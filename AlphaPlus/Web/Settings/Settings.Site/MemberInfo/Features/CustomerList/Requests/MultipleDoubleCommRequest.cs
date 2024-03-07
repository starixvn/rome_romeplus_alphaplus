namespace AlphaPlus.Settings.Site.Features.CustomerList.Requests
{
    using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;
    using AlphaPlus.Settings.Site.Models;
    using AlphaPlus.Core.Models;

    public class MultipleDoubleCommRequest : FetchDownlinesListRequesBase, IRequest<MultipleDoubleCommViewModel>
    {
        private readonly IUser viewer;

        public MultipleDoubleCommRequest() : this(null){}

        public MultipleDoubleCommRequest(IUser viewer) : base(viewer)
        {
            this.viewer = viewer ?? new User(UserSession.Current);
        }

        public override bool IsValid() => !string.IsNullOrEmpty(SearchAccountIds) && viewer.LevelId == SearchAccountRoleId + 1;

        public string SearchAccountIds { get; set; }

        public byte SearchAccountRoleId { get; set; }
    }
}