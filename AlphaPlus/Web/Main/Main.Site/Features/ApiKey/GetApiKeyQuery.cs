using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Main.Site.Features.ApiKey
{
    public class GetApiKeyQuery : IRequest<EditApiKeyViewModel>
    {
        public GetApiKeyQuery() : this(null)
        {
        }

        public GetApiKeyQuery(IUserSession userSession)
        {
            userSession ??= new UserSessionExt();
            Language = userSession.Language;
        }

        public string Language { get; }

        public string Key { get; set; }
    }
}