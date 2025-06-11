using MediatR;

namespace Alpha.Handbook.Demo.Requests
{
    public class ChangeLanguageRequest : BaseHandbookRequest, IRequest<string>
    {
        public string Selected { get; set; }
    }
}
