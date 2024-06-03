using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.MemberInfo.API.Helpers;
using AlphaPlus.Settings.MemberInfo.API.Services;
using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Base;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Multiple
{
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
            => MicroSites.SettingUrl("~/AutoTransfer/UpdateMultipleAutoTransfer");

        public override Dictionary<string, string> QueryStringParameters
        {
            get
            {
                var condition = accountService.GetTransferOption(SearchAccount.Upline.Id, SearchAccount.Upline.CustomerLevelValue);

                return new Dictionary<string, string>
                {
                    ["SearchAccountIds"] = Request.GetData<string>("customerIds"),
                    ["Usernames"] = Request.GetData<string>("usernames"),
                    ["SearchAccountRoleId"] = SearchAccount.CustomerLevelValue.ToString(),
                    ["Func"] = "UpdateMultipleAutoTransfer",
                    ["UplineCondition"] = string.Join(",", TransferConditionHelper.GetTransferrableDatesFromCondition(condition)),
                    ["Func"] = "UpdateMultipleAutoTransfer",
                    ["Old_TransferOption"] = condition,
                };
            }
        }
    }
}