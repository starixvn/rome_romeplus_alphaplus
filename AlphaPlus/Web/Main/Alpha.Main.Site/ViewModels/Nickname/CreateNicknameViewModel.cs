using AlphaPlus.Core.Models;
using Alpha.Main.AppResources;
using Alpha.Main.Site.Helpers;
using Alpha.Main.Models;
using AlphaPlus.Core.Helpers;

namespace Alpha.Main.Site.ViewModels.Nickname
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

        public string NicknameEffectedFrom
            => string.Format(Resources.NicknameEffectedFrom, nicknameChecker.ForceLoginDate);

        public bool IsRecommendLoginByNickname
            => nicknameChecker.IsRecommendLoginByNickname;

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