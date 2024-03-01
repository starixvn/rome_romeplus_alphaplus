namespace AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels.Products
{
    using AlphaPlus.Core.Helpers;
    using AlphaPlus.Settings.MemberInfo.API.Helpers;
    using AlphaPlus.Settings.MemberInfo.API.Requests;
    using AlphaPlus.Settings.MemberInfo.API.Services;
    using AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels.Base;
    using System.Collections.Generic;

    public class AutoTransferMultipleViewModel : SettingItemViewModel, ISettingItemMultipleViewModel
    {
        private new const int DisplayOrder = 1005;
        private readonly AccountService accountService;

        public AutoTransferMultipleViewModel(ISearchAccountInfoRequest request, AccountService accountService) : base(request, DisplayOrder)
        {
            this.accountService = accountService;
        }

        public override string Text => Request.GetResource("EditMultipleAutoTransferText");

        protected override string BaseAddress
            => MicroSites.TransferUrl("~/AutoTransfer/UpdateMultipleAutoTransfer");

        public override Dictionary<string, string> QueryStringParamaters
        {
            get
            {
                var condition = accountService.GetTransferOption(SearchAccount.Upline.Id, SearchAccount.Upline.CusomerLevelValue);

                return new Dictionary<string, string>
                {
                    ["SearchAccountIds"] = Request.GetData<string>("customerIds"),
                    ["Usernames"] = Request.GetData<string>("usernames"),
                    ["SearchAccountRoleId"] = SearchAccount.CusomerLevelValue.ToString(),
                    ["Func"] = "UpdateMultipleAutoTransfer",
                    ["UplineCondition"] = string.Join(",", TransferConditionHelper.GetTransferrableDatesFromCondition(condition)),
                    ["Func"] = "UpdateMultipleAutoTransfer",
                    ["Old_TransferOption"] = condition,
                };
            }
        }
    }
}