using MediatR;
using AlphaPlus.Features.Site.Handbook.ViewModels;

namespace AlphaPlus.Features.Site.Handbook.Requests
{
    public class HomeRequest : BaseHandbookRequest, IRequest<HomeViewModel>
    {
    }
}
