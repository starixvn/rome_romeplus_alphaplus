using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.CopyAccount.Request;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;

namespace AlphaPlus.Settings.Site.Features.CopyAccount.ViewModels
{
    public class CopyAccountViewModel
    {
        public CopyAccountViewModel(CopyAccountQuery request, string usernameSuffix, string hashKey)
        {
            ActorLevel = request.ViewerLevel;
            ActorUsername = request.CustName;
            UsernameSuffix = usernameSuffix;
            HashKey = hashKey;
            FirstChar = UsernameSuffix.Substring(0, 1).ToUpper();
            SecondChar = UsernameSuffix.Substring(1, 1).ToUpper();
            ThirdChar = request.ViewerLevel.IsAgentLevel() ? UsernameSuffix.Substring(2, 1).ToUpper() : string.Empty;
            Language = request.Language;
            IsDarkMode = request.IsDarkMode;
        }

        public CopyAccountViewModel(bool isLimitNewDownline, string errorMessage, CopyAccountQuery request)
        {
            IsLimitNewDownline = isLimitNewDownline;
            ErrorMessage = errorMessage;
            ActorLevel = request.ViewerLevel;
            Language = request.Language;
            IsDarkMode = request.IsDarkMode;
        }

        public UserLevel ActorLevel { get; }

        public string ActorUsername { get; }

        public string UsernameSuffix { get; }

        public string[] NameSuffix => [
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J",
            "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T",
            "U", "V", "W", "X", "Y", "Z" ];

        public string ThirdChar { get; }

        public string SecondChar { get; }

        public string FirstChar { get; }

        public string ErrorMessage { get; }

        public bool IsLimitNewDownline { get; }

        public string HashKey { get; }

        public bool IsCreateMember => ActorLevel.IsAgentLevel();

        public string PageTitle => ActorLevel.IsSuperLevel() ? Resources.AddNewMaster : (ActorLevel.IsMasterLevel() ? Resources.AddNewAgent : Resources.AddNewMember);

        public string Language { get; }

        public bool IsDarkMode { get; }

        public string GetUserGuideLink()
        {
            if (AppSettings.SupportedNewHandbook)
            {
                var handbookPath = UserLevel.GetDisplayName(ActorLevel.GetDirectDownLineLevel().Value).ToLowerInvariant();
                return MicroSites.FeatureUrl($"~/Default/GetBySlug?slug=create-new-{handbookPath}");
            }

            return AppSettings.IsSiteLabel789y
                ? MicroSites.FeatureUrl("~/StaticMessage/Guide?id=179159")
                : MicroSites.FeatureUrl("~/Index.aspx?linktomessagesite=179159");
        }
    }
}