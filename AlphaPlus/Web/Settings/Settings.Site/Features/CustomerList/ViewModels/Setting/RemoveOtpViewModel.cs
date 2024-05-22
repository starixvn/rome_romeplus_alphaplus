using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Base;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting
{
    public class RemoveOtpViewModel : SettingItemViewModel
    {
        public RemoveOtpViewModel(ISearchAccountInfoRequest request, int displayOrder = 99999)
            : base(request, displayOrder)

        {
        }

        protected override string BaseAddress => MicroSites.MainUrl("~/Otp/ResetOtp");

        public override Dictionary<string, string> QueryStringParamaters
           => new()
           {
               ["custId"] = SearchAccount.Id.ToString(),
               ["username"] = SearchAccount.Username
           };

        public override bool Visible =>
            base.Visible &&
            SearchAccount.IsNotMember &&
            Request.GetData<bool>(DataKey.Actor.HasUpdateAccountPermission);

        public override bool AddEditTextToPopupTitle => false;
    }
}