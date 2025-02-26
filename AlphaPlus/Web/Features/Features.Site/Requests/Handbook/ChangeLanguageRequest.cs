using MediatR;

namespace AlphaPlus.Features.Site.Handbook.Requests
{
    public class ChangeLanguageRequest : BaseHandbookRequest, IRequest<string>
    {
        public string Selected { get; set; }
    }
}
