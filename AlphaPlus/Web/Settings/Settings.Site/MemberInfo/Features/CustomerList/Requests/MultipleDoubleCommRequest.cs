namespace AlphaPlus.Settings.Site.Features.CustomerList.Requests
{
    using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using MediatR;

    public class MultipleDoubleCommRequest : FetchDownlinesListRequesBase, IRequest<MultipleDoubleCommViewModel>
    {
        private readonly IUser viewer;

        public MultipleDoubleCommRequest(IUser viewer) : base(viewer)
        {
            this.viewer = viewer;
        }

        public override bool IsValid() => !string.IsNullOrEmpty(SearchAccountIds) && viewer.LevelId == SearchAccountRoleId + 1;

        public string SearchAccountIds { get; set; }

        public byte SearchAccountRoleId { get; set; }
    }
}