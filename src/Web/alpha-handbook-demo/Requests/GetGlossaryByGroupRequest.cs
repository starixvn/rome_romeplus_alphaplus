using MediatR;
using Alpha.Handbook.Demo.ViewModels;

namespace Alpha.Handbook.Demo.Requests
{
    public class GetGlossaryByGroupRequest : BaseHandbookRequest, IRequest<GlossaryViewModel>
    {
        public string Group { get; set; }
    }
}
