using AlphaPlus.Core.Models;
using MediatR;

namespace Alpha.Main.Site.Features.Default
{
    public class BrowserQuery(IUserSession userSession) : IRequest<BrowserViewModel>
    {
        private readonly IUserSession userSession = userSession;

        public byte UserLevelId => userSession.RoleId;
    }
}