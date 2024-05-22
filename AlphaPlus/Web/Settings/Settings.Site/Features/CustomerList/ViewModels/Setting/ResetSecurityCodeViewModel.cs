using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Base;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting
{
    public class ResetSecurityCodeViewModel : SettingItemViewModel
    {
        public ResetSecurityCodeViewModel(ISearchAccountInfoRequest request, int displayOrder = 99998)
            : base(request, displayOrder)
        {
        }

        protected override string BaseAddress => MicroSites.MainUrl("~/SecurityCode/ResetSecurityCode");

        public override Dictionary<string, string> QueryStringParamaters
          => new()
          {
              ["custId"] = SearchAccount.Id.ToString(),
              ["username"] = SearchAccount.Username,
              ["popup"] = "0",
          };

        public override bool Visible =>
            base.Visible &&
            SearchAccount.IsNotMember &&
            Request.GetData<bool>(DataKey.Actor.HasUpdateAccountPermission) &&
            Request.GetData<bool>(DataKey.SearchAccount.IsEnableSecurityCode);

        public override bool AddEditTextToPopupTitle => false;
    }
}