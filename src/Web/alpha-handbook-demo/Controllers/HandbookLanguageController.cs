using MediatR;
using Alpha.Handbook.Demo.Requests;

namespace Alpha.Handbook.Demo.Controllers
{
    public class HandbookLanguageController : BaseController
    {
        public HandbookLanguageController(IMediator mediator) : base(mediator)
        {
        }

        public async Task Change(ChangeLanguageRequest request)
        {
           await mediator.Send(request);
        }
    }
}