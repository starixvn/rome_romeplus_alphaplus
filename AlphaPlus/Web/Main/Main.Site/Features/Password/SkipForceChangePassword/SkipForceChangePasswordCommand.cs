using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.Main.Site.Features.Password
{
    public class SkipForceChangePasswordCommand : IRequest<Result>
    {
        private readonly IUserSession userSession;

        public SkipForceChangePasswordCommand() : this(null)
        {
        }

        internal SkipForceChangePasswordCommand(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public int CustId => userSession.CustId;

        public int SubAccId => userSession.SubAccId;

        public string Language => userSession.LangKey;

        public string IP => userSession.UserLoginIp;

        public bool IsShowSkipForceChangePassword => userSession.IsShowSkipForceChangePassword;
    }
}