using AlphaPlus.Settings.MemberInfo.API.Services;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Status.Base;
using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
using AlphaPlus.Shared.AppResources.Settings.CustomerList;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Status
{
    public class ConfirmAllowAutoTransferViewModel : ConfirmStatusItemViewModelBase
    {
        private readonly IAccountService accountService;

        public ConfirmAllowAutoTransferViewModel(
            ISearchAccountInfoRequest request,
            IAccountService accountService,
            int displayOrder = 7) : base(request, displayOrder)
        {
            this.accountService = accountService;
        }

        public override bool Visible => Request.SupportAutoTransfer;

        public override bool Disabled
            => base.Disabled || Request.ActorIsNotEditingDirectDownline;

        public override bool Checked => Request.GetData<bool>(DataKey.SearchAccount.IsAllowAutoTransfer);

        public override bool ReloadCustomerList => true;

        public override bool ShowInCustomerList => true;

        public override bool IsPopup => true;

        public override string PopupHeaderText => Lang.Update;

        internal override string StatusName => DataKey.StatusMapping.AutoTransfer;

        internal override bool StatusValue => !Checked;

        public override string Url
        {
            get
            {
                var uplineCondition = accountService.GetTransferOption(SearchAccount.Upline.Id, SearchAccount.Upline.CustomerLevelValue);
                var url = "~/autotransfer/UpdateSingleAutoTransfer?";
                url += "CustId=" + SearchAccount.Id.ToString();
                url += "&CustName=" + SearchAccount.Username.ToString();
                url += "&State=" + (StatusValue ? "on" : "off");
                url += "&Old_TransferOption=" + uplineCondition;

                return MicroSites.SettingUrl(url);
            }
        }

        internal new IDictionary<string, object> Params
        {
            get
            {
                var uplineCondition = accountService.GetTransferOption(SearchAccount.Upline.Id, SearchAccount.Upline.CustomerLevelValue);
                return new Dictionary<string, object>
                    {
                        {"CustId", SearchAccount.Id.ToString()},
                        {"CustName", SearchAccount.Username.ToString()},
                        {"State", StatusValue? "on" : "off"},
                        { "Old_TransferOption", uplineCondition },
                    };
            }
        }


        public new string JsonParams =>
            string.Format("{0}{1}{2}", "{", string.Join(",", Params.Select(x => $"\"{x.Key}\":\"{x.Value.ToString()}\"")), "}");

        public override string ConfirmMessage => "ShowCheckboxOnCustInfo_AutoTransfer";
    }
}