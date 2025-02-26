using AlphaPlus.Settings.Site.Features.CustomerList.Models;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.CustomerList.Requests
{
    public class CustomerListRequest : FetchDownlinesListRequestBase, IRequest<CustomerListViewModel>
    {
        public IEnumerable<CustInfo> BreadcrumbLevel { get; set; } = [];
    }
}