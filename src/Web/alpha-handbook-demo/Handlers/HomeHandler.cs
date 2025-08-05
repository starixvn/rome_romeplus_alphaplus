using Alpha.Handbook.Demo.Requests;
using Alpha.Handbook.Demo.ViewModels;
using MediatR;

namespace Alpha.Handbook.Demo.Handlers
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