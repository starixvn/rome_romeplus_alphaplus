using MediatR;

namespace AlphaPlus.Main.Site.Features.WhiteLabel
{
    public class WhiteLabelSSORequest : IRequest<WhiteLabelSSOResponseViewModel>
    {
        public WhiteLabelSSORequest(string referenceId, string browserUserAgent, string userIp)
        {
            ReferenceId = referenceId;
            BrowserUserAgent = browserUserAgent;
            UserIp = userIp;
        }

        public string ReferenceId { get; }

        public string BrowserUserAgent { get; }

        public string UserIp { get; }
    }
}