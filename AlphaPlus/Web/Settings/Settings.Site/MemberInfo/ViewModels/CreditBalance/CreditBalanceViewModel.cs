namespace AlphaPlus.Settings.Site.ViewModels.CreditBalance
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using AlphaPlus.Core.Helpers;
    using AlphaPlus.Settings.MemberInfo.Domain.AppResources;
    using AlphaPlus.Settings.MemberInfo.Domain.Constants;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Models.CreditBalance;
    using AlphaPlus.Settings.MemberInfo.Domain.Models.Winloss;
    using Requests.CreditBalance;
    using Sunplus.Agent.Utilities;

    public class CreditBalanceViewModel
    {
        public CreditBalanceViewModel(CreditBalanceQuery message, EnteredSportResult enteredSportResult, bool isAllDayBalTransfer)
            : this(new PagedCreditBalance(message.PageIndex, message.PageSize, 0, Enumerable.Empty<CreditBalanceItem>()),
                  null,
                  message,
                  enteredSportResult,
                  isAllDayBalTransfer)
        {
        }

        public CreditBalanceViewModel(
            PagedCreditBalance pagedCreditBalances,
            string transferCondition,
            CreditBalanceQuery message,
            EnteredSportResult enteredSportResult,
            bool isAllDayBalTransfer)
        {
            LegendView = $"Legend.{message.Language}";
            PagedCreditBalances = pagedCreditBalances;
            TransferCondition = transferCondition;

            CustId = message.CustId;
            CustLevelId = message.CustLevelId;
            CustLevel = message.CustLevel;
            UserName = message.UserName;
            StatusId = message.StatusId.Value;

            DownlineLevel = message.CustLevel.GetDirectDownLineLevel();
            IsViewingMemberList = DownlineLevel.IsMemberLevel();
            ActorIsAgent = message.ActorLevel.IsAgentLevel();
            CanUpdateAccount = message.CanUpdateAccount;
            var showTransferPopup = !isAllDayBalTransfer
                && ActorIsAgent
                && message.CanTransfer;
            CanTransferOnBalanceColumn = showTransferPopup && message.IsAllowedTotalBalanceTransfer;
            CanTransferOnYesterdayBalanceColumn = showTransferPopup && !message.IsAllowedTotalBalanceTransfer;
            CanViewBetListsFromOutstanding = IsViewingMemberList && message.CanViewBetLists;
            IsShowDateTimeColumn = message.LimitedDateToShowInfo == default(DateTime);
            CookiePrefix = Encryption.PearsonEncode(string.Concat(message.LoggedInUserId, message.ActorId, message.SubAccId, "CreditBalance"));
            EnteredSportResult = enteredSportResult;
        }

        public string LegendView { get; }

        public PagedCreditBalance PagedCreditBalances { get; }

        public string TransferCondition { get; }

        public int CustId { get; }

        public byte CustLevelId { get; }

        public UserLevel CustLevel { get; }

        public string UserName { get; }

        public byte StatusId { get; }

        public IEnumerable<SelectListItem> Status
        {
            get
            {
                yield return BuildSelectListItem(Resources.All, StatusIdentity.All);
                yield return BuildSelectListItem(Resources.Open, StatusIdentity.Open);
                yield return BuildSelectListItem(Resources.Suspended, StatusIdentity.Suspended);
                yield return BuildSelectListItem(Resources.Closed, StatusIdentity.Closed);
                yield return BuildSelectListItem(Resources.Disabled, StatusIdentity.Disabled);
            }
        }

        private SelectListItem BuildSelectListItem(string text, byte value)
            => new SelectListItem { Text = text, Value = value.ToString(), Selected = value == StatusId };

        public UserLevel DownlineLevel { get; }

        public bool IsViewingMemberList { get; }

        public bool ActorIsAgent { get; }

        public bool CanUpdateAccount { get; }

        public bool CanTransferOnBalanceColumn { get; }

        public bool CanTransferOnYesterdayBalanceColumn { get; }

        public bool CanViewBetListsFromOutstanding { get; }

        public bool IsShowDateTimeColumn { get; }

        public string CookiePrefix { get; }

        public string GetRunningBetListLink(CreditBalanceItem item)
            => Helpers.UrlHelper.MapBetListPath($"~/RunningBetList/Index?custId={item.CustId}&custName={item.UserName}&getAll=true");

        public string OldMainRootPath => MicroSites.Settings();

        public string GivenCreditLink => MicroSites.AccountInfoUrl($"~/SearchAccount/GivenCredit");

        public string GetMemberBalanceInfoLink(string custName)
            => MicroSites.AccountInfoUrl($"~/SearchAccount/GetMemberBalanceInfo?custname={custName}");

        public EnteredSportResult EnteredSportResult { get; }
    }
}