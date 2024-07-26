using AlphaPlus.Settings.MemberInfo.API.Helpers;
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

        public override string ConfirmMessage
        {
            get => Checked ? GetConfirmMessage_Disable() : GetConfirmMessage_Enable();
        }

        internal override string SuffixMessage
        {
            get
            {
                return Checked ? "InCaseDisabledMemberMessage" : "InCaseEnabledMemberMessage";
            }
        }

        internal override string StatusName => DataKey.StatusMapping.AutoTransfer;

        internal override bool StatusValue => !Checked;

        public override string Url => MicroSites.SettingUrl("~/autotransfer/update");

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
                        {"UseDifferentSite", 1},
                        {"Func", "UpdateAutoTransfer"},
                        { "Old_TransferOption", uplineCondition },
                    };
            }
        }


        public new string JsonParams =>
            string.Format("{0}{1}{2}", "{", string.Join(",", Params.Select(x => $"\"{x.Key}\":\"{x.Value.ToString()}\"")), "}");

        private string GetConfirmMessage_Disable()
        {
            var hintPath = Lang.ResourceManager.GetString("AutoTransferPageHint_" + SearchAccount.CustomerLevel.DisplayName);
            var message = string.Format(Lang.ConfirmAllowAutoTransferViewModelInCaseDisabledMemberMessage_Message, SearchAccount.Username.ToString());

            message += "<ul class='ul-confirm'>";
            message += "<li class='clear-schedule-note'>" + Lang.AutoTransfer_ClearSchedule + "</li>";
            message += "<li class='tip'><span class='icon-lightbulb-on'></span>";
            message += string.Format(Lang.ConfirmAllowAutoTransferViewModelInCaseDisabledMemberMessage_Tip, hintPath);
            message += "</li></ul>";

            return message;
        }

        private string GetConfirmMessage_Enable()
        {
            var hintPath = Lang.ResourceManager.GetString("AutoTransferPageHint_" + SearchAccount.CustomerLevel.DisplayName);
            var uplineCondition = accountService.GetTransferOption(SearchAccount.Upline.Id, SearchAccount.Upline.CustomerLevelValue);
            var transferrableDates = TransferConditionHelper.GetTransferrableDatesFromCondition(uplineCondition);

            string[] dates = { Lang.Sun, Lang.Mon, Lang.Tue, Lang.Wed, Lang.Thurs, Lang.Fri, Lang.Sat };
            var schedule = Request.GetData<string>(DataKey.SearchAccount.ScheduleWeekDays);
            var selectedDates = string.IsNullOrWhiteSpace(schedule)
                ? transferrableDates
                : TransferConditionHelper.GetTransferrableDatesFromSchedule(schedule);

            var scheduleChecks = "<span class='spn-schedule'>" +
                "<input id='opt-schedule-all' type='checkbox' class='opt-schedule-all' "
                    + (selectedDates.Count == transferrableDates.Count ? " checked='checked'" : "") + " />" + Lang.SelectAll + "</span>";

            foreach (var d in transferrableDates)
            {
                scheduleChecks += "<span class='spn-schedule'>" +
                    "<input type='checkbox' class='opt-schedule'" +
                        (selectedDates.Any(s => s == d) ? " checked='checked'" : "") + " data-val='" + d + "' />" + dates[d - 1] + "</span>";
            }

            var message = string.Format(Lang.ConfirmAllowAutoTransferViewModelInCaseEnabledMemberMessage_Message, SearchAccount.Username.ToString());
            message += "<ul class='ul-confirm'>";
            message += "<li>" + Lang.AutoTransfer_AffectTime + "</li>";
            message += "<li>" + Lang.AutoTransfer_SetSchedule + "</li>";
            message += "<li class='li-schedule'>" + scheduleChecks + "</li>";
            message += "<li class='tip'><span class='icon-lightbulb-on'></span>";
            message += string.Format(Lang.ConfirmAllowAutoTransferViewModelInCaseEnabledMemberMessage_Tip, hintPath);
            message += "</li></ul>";

            return message;
        }
    }
}