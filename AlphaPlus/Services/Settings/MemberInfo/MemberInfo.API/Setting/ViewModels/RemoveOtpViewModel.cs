namespace AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels
{
    using AlphaPlus.Settings.MemberInfo.API.Requests;
    using Base;
    using System.Collections.Generic;

    public class RemoveOtpViewModel : SettingItemViewModel
    {
        public RemoveOtpViewModel(ISearchAccountInfoRequest request, int displayOrder = 99999)
            : base(request, displayOrder)

        {
        }

        protected override string BaseAddress => $"{MainUrl}Otp/ResetOtp";

        public override Dictionary<string, string> QueryStringParamaters
           => new Dictionary<string, string>
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