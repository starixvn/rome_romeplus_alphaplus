using AlphaPlus.Core.Models;
using AlphaPlus.Web.Models;
using MediatR;

#pragma warning disable 612, 618

namespace Alpha.Main.Site.Requests
{
    public class BrowserQuery : IRequest<BrowserViewModel>
    {
        private readonly IUserSession userSession;

        public BrowserQuery(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public byte UserLevelId => userSession.RoleId;
    }
}