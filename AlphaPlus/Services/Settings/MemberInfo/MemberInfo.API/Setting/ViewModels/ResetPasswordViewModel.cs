namespace AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels
{
    using AlphaPlus.Settings.MemberInfo.API.Requests;
    using Alpha.Products.Core.Shared;
    using Base;
    using System.Collections.Generic;

    public class ResetPasswordViewModel : SettingItemViewModel
    {
        public ResetPasswordViewModel(ISearchAccountInfoRequest request, int displayOrder = 6)
            : base(request, displayOrder)
        {
        }

        protected override string BaseAddress => $"{MainUrl}Password/ResetMainAccountPassword";

        public override Dictionary<string, string> QueryStringParamaters
          => new Dictionary<string, string>
          {
              ["custId"] = SearchAccount.Id.ToString(),
              ["username"] = SearchAccount.Username,
              ["custRoleId"] = SearchAccount.CusomerLevelValue.ToString()
          };

        public override bool Visible => Actor.IsMainAccount || Request.GetData<bool>(DataKey.Actor.HasUpdateAccountPermission);

        public override bool AddEditTextToPopupTitle => false;
    }
}