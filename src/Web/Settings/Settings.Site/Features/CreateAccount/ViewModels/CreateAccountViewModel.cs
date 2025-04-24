using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using AlphaPlus.Core.Models;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Helpers;

namespace AlphaPlus.Settings.Site.Features.CreateAccount.ViewModels
{
    public class CreateAccountViewModel
    {
        public CreateAccountViewModel(
            string language, bool isDarkMode, bool isLimitNewDownline = false, string errorMessage = "",
            byte levelId = 0, string secretKey = "", string suggestUsername = "", int custId = 0)
        {
            Language = language;
            IsDarkMode = isDarkMode;
            IsLimitNewDownline = isLimitNewDownline;
            ErrorMessage = errorMessage;
            IsCreateMember = levelId == UserLevel.Agent.Value;
            var userLevel = Enumeration.FromValue<UserLevel>(levelId);
            CustLevel = userLevel.DisplayName;
            SecretKey = secretKey;
            PageTitle = userLevel.IsSuperLevel()
                ? Resources.AddNewMaster
                : (userLevel.IsMasterLevel() ? Resources.AddNewAgent : Resources.AddNewMember);
            Params = JsonConvert.SerializeObject(new { uplineId = custId, suggestUsername, uplineRoleId = levelId },
                new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
           ActorLevel = userLevel;
        }

        public string ErrorMessage { get; }

        public bool IsLimitNewDownline { get; }

        public bool IsCreateMember { get; }

        public UserLevel ActorLevel { get; }

        public string CustLevel { get; }

        public string PageTitle { get; }

        public string SecretKey { get; }

        public string Language { get; }

        public bool IsDarkMode { get; }

        public string Params { get; }

        public string GetUserGuideLink()
        {
            if (AppSettings.SupportedNewHandbook)
            {
                var handbookPath = UserLevel.GetDisplayName(ActorLevel.GetDirectDownLineLevel().Value).ToLowerInvariant();
                return MicroSites.FeatureUrl($"~/HandbookDefault/GetBySlug?slug=create-new-{handbookPath}");
            }

            return AppSettings.IsSiteLabel789y
                ? MicroSites.FeatureUrl("~/StaticMessage/Guide?id=179159")
                : MicroSites.FeatureUrl("~/Index.aspx?linktomessagesite=179159");
        }
    }
}