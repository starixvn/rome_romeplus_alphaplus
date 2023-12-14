using Alpha.Main.Models.ApiKey;
using AlphaPlus.Core.Models;
using MediatR;

namespace Alpha.Main.Site.Requests.ApiKey
{
    public class CreateApiKeyCommand : IRequest<CreateApiKeyResult>
    {
        public CreateApiKeyCommand() : this(null)
        {
        }

        public CreateApiKeyCommand(IUserSession userSession)
        {
            userSession = userSession ?? new UserSessionExt();
            CustId = userSession.CustId;
            IP = userSession.UserLoginIp;
            Language = userSession.LangKey;
        }

        public int CustId { get; }

        public string ApiName { get; set; }

        public string PassPhrase { get; set; }

        public string Scopes { get; set; }

        public string IP { get; }

        public string Language { get; }

        public DateTime? ExpirationDate { get; set; }
    }
}