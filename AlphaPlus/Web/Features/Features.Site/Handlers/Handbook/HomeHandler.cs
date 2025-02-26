using AlphaPlus.Features.Site.Handbook.Requests;
using AlphaPlus.Features.Site.Handbook.ViewModels;
using MediatR;

namespace AlphaPlus.Features.Site.Handbook.Handlers
{
    public class HomeHandler :
        IRequestHandler<HomeRequest, HomeViewModel>
    {
        public Task<HomeViewModel> Handle(HomeRequest request, CancellationToken cancellation)
        {
            var viewModel = new HomeViewModel();

            return Task.FromResult(viewModel);
        }
    }
}