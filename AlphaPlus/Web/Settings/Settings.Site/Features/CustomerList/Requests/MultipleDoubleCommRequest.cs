using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.CustomerList.Requests
{
    public class MultipleDoubleCommRequest : FetchDownlinesListRequestBase, IRequest<MultipleDoubleCommViewModel>
    {
        public override bool IsValid() => !string.IsNullOrEmpty(SearchAccountIds) && viewer.LevelId == SearchAccountRoleId + 1;

        public string SearchAccountIds { get; set; }

        public byte SearchAccountRoleId { get; set; }

        public string Usernames { get; set; }
    }
}