using AlphaPlus.Core.Models;
using MediatR;

namespace Alpha.Main.Site.Features.ApiKey
{
    public class RevokeApiKeyCommand : IRequest<Result>
    {
        public RevokeApiKeyCommand() : this(null)
        {
        }

        public RevokeApiKeyCommand(IUserSession userSession)
        {
            userSession ??= new UserSessionExt();
            IP = userSession.UserLoginIp;
        }

        public string Key { get; set; }

        public string IP { get; }
    }
}