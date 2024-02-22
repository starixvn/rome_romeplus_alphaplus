using AlphaPlus.Core.Models;
using MediatR;

namespace Alpha.Main.Site.Features.UMMessage
{
    public class UMMessageQuery : IRequest<IEnumerable<UMMessageDTO>>
    {
        private readonly IUserSession userSession;

        public UMMessageQuery()
            : this(new UserSessionExt())
        {
        }

        public UMMessageQuery(IUserSession userSession)
        {
            this.userSession = userSession;
        }

        public int CustId => userSession.CustId;

        public string Site => userSession.Site;

        public int CurrencyId => userSession.CurrencyId;

        public int SuperId => userSession.SRecommendId;

        public bool EnableUMAnnouncementPopup
            => AppSettings.EnableUMAnnouncementPopup && !userSession.IsProtectedMode;
    }
}