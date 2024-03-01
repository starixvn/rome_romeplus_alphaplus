using Alpha.MemberInfo.Site.Features.CustomerList.Requests;

namespace Alpha.MemberInfo.Site.Features.CustomerList.ViewModels
{
    public class MultipleDoubleCommViewModel
    {
        public MultipleDoubleCommViewModel(MultipleDoubleCommRequest request)
        {
            SearchAccountRoleId = request.SearchAccountRoleId;
            SearchAccountIds = request.SearchAccountIds;
        }

        public byte SearchAccountRoleId { get; }

        public string SearchAccountIds { get; }
    }
}