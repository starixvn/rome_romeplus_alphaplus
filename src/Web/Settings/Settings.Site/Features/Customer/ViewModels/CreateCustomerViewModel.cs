using System.Text.Json;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Customer.Requests;
using AlphaPlus.Shared.AppResources.Settings;

namespace AlphaPlus.Settings.Site.Features.Customer.ViewModels
{
    public class CreateCustomerViewModel : CustomerBaseViewModel
    {
        public CreateCustomerViewModel(CreateCustomerQuery query, bool isLimitNewDownline, string message)
            : this(query, null, string.Empty, string.Empty, string.Empty)
        {
            IsLimitNewDownline = isLimitNewDownline;
            Message = message;
        }

        public CreateCustomerViewModel(
            CreateCustomerQuery query,
            AppConfiguration appConfig,
            string cndUri,
            string assetsVersion,
            string secretKey,
            string suggestUsername = "",
            int productGroupId = 1)
            : base(appConfig, cndUri, assetsVersion)
        {
            SecretKey = secretKey;
            Params = JsonSerializer.Serialize(new { uplineId = query.CustId, uplineRoleId = query.CustLevelId, suggestUsername, productGroupId });
            ActorId = query.ViewerId;
            SubAccId = query.SubAccId;
            ActorLevel = query.ViewerLevel;
            Language = query.Language;
            IsDarkMode = query.IsDarkMode;
        }

        public string SecretKey { get; }

        public int SubAccId { get; }

        public int ActorId { get; }

        public UserLevel ActorLevel { get; }

        public string PageTitle => ActorLevel.IsSuperLevel() ? Resources.AddNewMaster : (ActorLevel.IsMasterLevel() ? Resources.AddNewAgent : Resources.AddNewMember);

        public bool IsCreateMember => ActorLevel.IsAgentLevel();

        public string Language { get; }

        public bool IsDarkMode { get; }

        public bool IsLimitNewDownline { get; }

        public string Message { get; }

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