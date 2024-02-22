using AlphaPlus.Core.Models;
using MediatR;

namespace Alpha.Main.Site.Features.OTP
{
    public class SetUpOtpQuery : IRequest<OtpViewModel>
    {
        private readonly IUserSession userSession;

        public SetUpOtpQuery() : this(null)
        {
        }

        internal SetUpOtpQuery(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
            CustId = this.userSession.CustId;
            SubAccountId = this.userSession.SubAccId;
            CustLevel = Enumeration.FromValue<UserLevel>(this.userSession.RoleId);
            Language = this.userSession.Language;
        }

        public int CustId { get; }

        public int SubAccountId { get; }

        public string Username
        {
            get; set;
        }

        public UserLevel CustLevel { get; }

        public string Language { get; }

        public bool IsOfferProtectedMode => userSession.IsOfferProtectedMode;
    }
}