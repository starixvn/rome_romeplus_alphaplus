using AlphaPlus.Settings.Site.Features.CustomerList.Requests;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels
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