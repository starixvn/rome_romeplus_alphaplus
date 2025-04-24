using MediatR;

namespace AlphaPlus.Features.Site.Handbook.Requests
{
    public class GetImageByNameRequest : BaseHandbookRequest, IRequest<string>
    {
        public string Name { get; set; }
    }
}
