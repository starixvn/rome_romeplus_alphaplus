namespace AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels
{
    using System.Collections.Generic;
    using AlphaPlus.Settings.MemberInfo.API.Requests;
    using Base;

    public class CommissionViewModel : SettingItemViewModel
    {
        private const string EditCommissionUrl = "_MemberInfo/EditCommission/EditCommission.aspx";
        private const string EditMemberCommisionUrl = "_MemberInfo/EditCommission/EditMemberCommission.aspx";

        public CommissionViewModel(ISearchAccountInfoRequest request, int displayOrder = 4)
            : base(request, displayOrder)
        {
        }

        protected override string BaseAddress 
            => OldMainUrl + (Request.SearchAccount.IsMember ? EditMemberCommisionUrl : EditCommissionUrl);

        public override Dictionary<string, string> QueryStringParamaters
        {
            get
            {
                var query = new Dictionary<string, string>
                {
                    ["custId"] = SearchAccount.Id.ToString(),
                    ["username"] = SearchAccount.Username
                };

                if (Request.SearchAccount.IsMember)
                {
                    query.Add("roleId", SearchAccount.CustomerLevel.Value.ToString());
                }

                return query;
            }
        }
    }
}