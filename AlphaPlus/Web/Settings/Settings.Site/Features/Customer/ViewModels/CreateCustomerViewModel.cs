using System.Text.Json;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Customer.Requests;
using AlphaPlus.Shared.AppResources.Settings;

namespace AlphaPlus.Settings.Site.Features.Customer.ViewModels
{
    public class CreateCustomerViewModel : CustomerBaseViewModel
    {
        public CreateCustomerViewModel(CreateCustomerQuery query, bool isLimitNewDownline, string message)
            : this(query, null, string.Empty, string.Empty)
        {
            IsLimitNewDownline = isLimitNewDownline;
            Message = message;
        }

        public CreateCustomerViewModel(
            CreateCustomerQuery query,
            AppConfiguration appConfig,
            string cndUri,
            string secretKey,
            string suggestUsername = "")
            : base(appConfig, cndUri)
        {
            SecretKey = secretKey;
            Params = JsonSerializer.Serialize(new { uplineId = query.CustId, uplineRoleId = query.CustLevelId, suggestUsername });
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
    }
}