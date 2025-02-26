using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Base;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting
{
    public class ResetPasswordViewModel : SettingItemViewModel
    {
        public ResetPasswordViewModel(ISearchAccountInfoRequest request, int displayOrder = 6)
            : base(request, displayOrder)
        {
        }

        protected override string BaseAddress => MicroSites.MainUrl($"~/Password/ResetMainAccountPassword");

        public override Dictionary<string, string> QueryStringParameters
          => new()
          {
              ["custId"] = SearchAccount.Id.ToString(),
              ["username"] = SearchAccount.Username,
              ["custRoleId"] = SearchAccount.CustomerLevelValue.ToString()
          };

        public override bool Visible => Actor.IsMainAccount || Request.GetData<bool>(DataKey.Actor.HasUpdateAccountPermission);

        public override bool AddEditTextToPopupTitle => false;
    }
}