using MediatR;
using Alpha.Handbook.Demo.ViewModels;

namespace Alpha.Handbook.Demo.Requests
{
    public class HomeRequest : BaseHandbookRequest, IRequest<HomeViewModel>
    {
    }
}
