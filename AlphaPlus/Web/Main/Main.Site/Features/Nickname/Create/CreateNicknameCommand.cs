using AlphaPlus.Core.Models;
using MediatR;
using AlphaPlus.Core.Helpers;

namespace AlphaPlus.Main.Site.Features.Nickname
{
    public class CreateNicknameCommand : IRequest<Result>
    {
        private readonly IUserSession userSession;
        private readonly INicknameChecker nicknameChecker;

        public CreateNicknameCommand(
            IUserSession userSession,
            INicknameChecker nicknameChecker = null)
        {
            this.userSession = userSession;
            this.nicknameChecker = nicknameChecker ?? new NicknameChecker(this.userSession);
        }

        public string Nickname { get; set; }

        public bool IsDisableUpdateNickname
            => !string.IsNullOrWhiteSpace(userSession.NickName);

        public int ActorId => userSession.CustId;

        public int SubAccountId => userSession.SubAccId;

        public string IP => userSession.UserLoginIp;

        public string LanguageKey => userSession.LangKey;

        public bool IsValid()
            => nicknameChecker.IsOfferForceLoginFlow;
    }
}