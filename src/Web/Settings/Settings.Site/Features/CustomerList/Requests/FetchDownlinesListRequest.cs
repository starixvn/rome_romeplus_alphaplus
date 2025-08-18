using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.CustomerList.Requests
{
    public class FetchDownlinesListRequest : FetchDownlinesListRequestBase, IRequest<CustomerListPagingViewModel>
    {
        public byte LevelType { get; set; }

        public byte ServiceType { get; set; } = 0;
    }
}