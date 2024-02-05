using AlphaPlus.Core.Models;
using Alpha.Main.Site.Helpers;
using AlphaPlus.Core.Helpers;

namespace Alpha.Main.Site.Features.Nickname
{
    public class CreateNicknameViewModel
    {
        private readonly IUserSession userSession;
        private readonly INicknameChecker nicknameChecker;

        public CreateNicknameViewModel(
            IUserSession userSession,
            string nickname = "",
            INicknameChecker nicknameChecker = null)
        {
            this.userSession = userSession;
            Nickname = string.IsNullOrWhiteSpace(this.userSession.NickName) ? nickname : this.userSession.NickName;
            this.nicknameChecker = nicknameChecker ?? new NicknameChecker(this.userSession);
        }

        public string Nickname { get; }

        public bool IsDisableUpdateNickname
            => !string.IsNullOrWhiteSpace(userSession.NickName);

        public string Username
            => userSession.UserName;

        public static string MainRootPath
            => UrlHelper.MapMainPath("~");

        public Result Result { get; set; }

        public bool IsValid()
            => nicknameChecker.IsOfferForceLoginFlow;
    }
}