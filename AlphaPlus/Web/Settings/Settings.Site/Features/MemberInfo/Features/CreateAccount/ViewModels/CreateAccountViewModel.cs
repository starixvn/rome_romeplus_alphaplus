using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using AlphaPlus.Settings.MemberInfo.Domain.Enumerations;
using AlphaPlus.Settings.MemberInfo.Domain.Models;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.CreateAccount.ViewModels
{
    public class CreateAccountViewModel
    {
        public CreateAccountViewModel(bool isLimitNewDownline = false, string errorMessage = "", byte levelId = 0, string secretKey="")
        {
            IsLimitNewDownline = isLimitNewDownline;
            ErrorMessage = errorMessage;
            IsCreateMember = levelId == UserLevel.Agent.Value;
            var userLevel = Enumeration.FromValue<UserLevel>(levelId);
            CustLevel = userLevel.DisplayName;
            SecretKey = secretKey;
            PageTitle = userLevel.IsSuperLevel()
                ? Resources.AddNewMaster
                : (userLevel.IsMasterLevel() ? Resources.AddNewAgent : Resources.AddNewMember);
        }

        public string ErrorMessage { get; }

        public bool IsLimitNewDownline { get; }

        public bool IsCreateMember { get; }

        public string CustLevel { get; }

        public string PageTitle { get; }

        public string SecretKey { get; }
    }
}