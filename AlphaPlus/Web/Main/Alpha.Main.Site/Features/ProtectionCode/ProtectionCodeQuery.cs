using AlphaPlus.Core.Models;
using MediatR;

namespace Alpha.Main.Site.Features.ProtectionCode
{
    public class ProtectionCodeQuery : IRequest<ProtectionCodeViewModel>
    {
        public ProtectionCodeQuery() : this(null)
        {
        }

        public ProtectionCodeQuery(IUserSession userSession)
        {
            userSession ??= new UserSessionExt();
            CustId = userSession.CustId;
            SubAccId = userSession.SubAccId;
            RoleId = userSession.RoleId;
            Language = userSession.Language;
            IsOfferProtectedMode = userSession.IsOfferProtectedMode;
        }

        public int CustId { get; }

        public int SubAccId { get; }

        public byte RoleId { get; }

        public string Language { get; }

        public bool IsOfferProtectedMode { get; }
    }
}