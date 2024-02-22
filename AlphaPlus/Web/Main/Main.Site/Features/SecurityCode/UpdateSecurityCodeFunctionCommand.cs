namespace AlphaPlus.Main.Site.Features.SecurityCode
{
    using AlphaPlus.Core.Models;
    using MediatR;

    public class UpdateSecurityCodeFunctionCommand : IRequest<Result>
    {
        private readonly IUserSession userSession;

        public UpdateSecurityCodeFunctionCommand() : this(null)
        {
        }

        public UpdateSecurityCodeFunctionCommand(IUserSession userSession)
        {
            this.userSession = userSession ?? new UserSessionExt();
        }

        public bool IsOn { get; set; }

        public string Function { get; set; }

        public int CustId => userSession.CustId;

        public int SubAccId => userSession.SubAccId;

        public string LangKey => userSession.LangKey;

        public string IP => userSession.UserLoginIp;
    }
}