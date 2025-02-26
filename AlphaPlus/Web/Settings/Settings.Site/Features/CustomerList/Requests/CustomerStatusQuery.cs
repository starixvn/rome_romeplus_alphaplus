using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.CustomerList.Requests
{
    public class CustomerStatusQuery : CustomerStatusQueryBase, IRequest<CustomerStatusViewModel>
    {
    }
}