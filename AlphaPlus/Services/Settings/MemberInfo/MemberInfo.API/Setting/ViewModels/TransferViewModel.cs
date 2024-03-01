namespace AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels
{
    using System.Collections.Generic;
    using AlphaPlus.Settings.MemberInfo.API.Requests;
    using Base;

    public class TransferViewModel : SettingItemViewModel
    {
        public TransferViewModel(ISearchAccountInfoRequest request, int displayOrder = 2)
            : base(request, displayOrder)
        {
        }

        protected override string BaseAddress 
            => $"{OldMainUrl}_MemberInfo/TransferCondition/EditTransferCondition.aspx";

        public override Dictionary<string, string> QueryStringParamaters
          => new Dictionary<string, string>
          {
              ["custId"] = SearchAccount.Id.ToString(),
              ["username"] = SearchAccount.Username
          };

        public override bool Visible
            => base.Visible
            && (SearchAccount.IsMaster
                || (SearchAccount.IsAgent
                     && Request.GetData<bool>(DataKey.Actor.CanEditDailyTransferOption)));
    }
}