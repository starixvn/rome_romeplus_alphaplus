using System.Text.Json;
using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Customer.Requests;

namespace AlphaPlus.Settings.Site.Features.Customer.ViewModels
{
    public class CreateDirectMemberViewModel : CustomerBaseViewModel
    {
        public CreateDirectMemberViewModel(CreateCustomerQuery query, bool isLimitNewDownline, string message)
                    : this(query, null, string.Empty, string.Empty, string.Empty)
        {
            IsLimitNewDownline = isLimitNewDownline;
            Message = message;
        }

        public CreateDirectMemberViewModel(
            CreateCustomerQuery query,
            AppConfiguration appConfig,
            string cndUri,
            string secretKey,
            string suggestUsername = "")
            : base(appConfig, cndUri)
        {
            SecretKey = secretKey;
            Params = JsonSerializer.Serialize(new
            {
                uplineId = query.CustId,
                uplineRoleId = query.CustLevelId,
                suggestUsername,
                productGroupId = 0,
                agentId = query.ViewerLevel.IsSuperLevel() ? -4 : -3
            });
            ActorLevel = query.ViewerLevel;
        }

        public bool IsLimitNewDownline { get; }

        public string Message { get; }

        public string SecretKey { get; }

        public UserLevel ActorLevel { get; }

    }
}