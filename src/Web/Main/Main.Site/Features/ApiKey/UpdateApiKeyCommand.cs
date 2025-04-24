using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Main.Site.Features.ApiKey
{
    public class UpdateApiKeyCommand : IRequest<Result>
    {
        public UpdateApiKeyCommand() : this(null)
        {
        }

        public UpdateApiKeyCommand(IUserSession userSession)
        {
            userSession ??= new UserSessionExt();
            CustId = userSession.CustId;
            IP = userSession.UserLoginIp;
            Language = userSession.LangKey;
        }

        public string ApiKey { get; set; }

        public int CustId { get; }

        public string ApiName { get; set; }

        public string Scopes { get; set; }

        public string IP { get; }

        public string Language { get; }

        public DateTime? ExpirationDate { get; set; }
    }
}