using AlphaPlus.Features.Site.Handbook.Requests;
using AlphaPlus.Features.Site.Handbook.ViewModels;
using MediatR;

namespace Alpha.HowToUse.Handbook.Handlers
{
    public class HomeHandler :
        IRequestHandler<HomeRequest, HomeViewModel>
    {
        public async Task<HomeViewModel> Handle(HomeRequest request, CancellationToken cancellation)
        {
            var viewModel = new HomeViewModel();

            return viewModel;
        }
    }
}