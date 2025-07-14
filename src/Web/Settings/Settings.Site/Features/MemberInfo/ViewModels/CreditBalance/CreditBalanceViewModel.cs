using AlphaPlus.Core.Configurations;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Core.Models;
using AlphaPlus.Core.Utils;
using AlphaPlus.Settings.MemberInfo.Domain.Constants;
using AlphaPlus.Settings.MemberInfo.Domain.Models.CreditBalance;
using AlphaPlus.Settings.MemberInfo.Domain.Models.Winloss;
using AlphaPlus.Settings.Site.Features.MemberInfo.Requests.CreditBalance;
using AlphaPlus.Shared.AppResources.Settings.MemberInfo;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.ViewModels.CreditBalance;

public class CreditBalanceViewModel
{
    public CreditBalanceViewModel(CreditBalanceQuery message, EnteredSportResult enteredSportResult, bool isAllDayBalTransfer) : this(
        new PagedCreditBalance(message.PageIndex, message.PageSize, 0, []),
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
        LegendView = GetLegendViewByLanguage(message.Language);
        PagedCreditBalances = pagedCreditBalances;
        TransferCondition = transferCondition;

        ActorLevel = message.ActorLevel;
        CustId = message.CustId;
        CustLevelId = message.CustLevelId;
        CustLevel = message.CustLevel;
        UserName = message.UserName;
        StatusId = message.StatusId.Value;

        DownlineLevel = message.CustLevel.GetDirectDownLineLevel();
        IsViewingMemberList = DownlineLevel.IsMemberLevel();
        ActorIsAgent = message.ActorLevel.IsAgentLevel();
        CanUpdateAccount = message.CanUpdateAccount;
        var showTransferPopup = !isAllDayBalTransfer && message.CanTransfer;
        CanTransferOnBalanceColumn = showTransferPopup && message.IsAllowedTotalBalanceTransfer;
        CanTransferOnYesterdayBalanceColumn = showTransferPopup && !message.IsAllowedTotalBalanceTransfer;
        CanViewBetLists = message.CanViewBetLists;
        IsShowDateTimeColumn = message.LimitedDateToShowInfo == default;
        CookiePrefix = new Encryption().PearsonEncode(string.Concat(message.UserId, message.ViewerId, message.SubAccId, "CreditBalance"));
        EnteredSportResult = enteredSportResult;
        ShowLevelType = message.CustLevel.IsNotAgentLevel();
        LevelType = message.LevelType;
        IsOfferedCashService = AppSettings.IsOfferedCashService(message.Site);
    }

    public bool IsOfferedCashService { get; }

    public string LegendView { get; }

    public PagedCreditBalance PagedCreditBalances { get; }

    public string TransferCondition { get; }

    public UserLevel ActorLevel { get; }

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
        => new() { Text = text, Value = value.ToString(), Selected = value == StatusId };

    public UserLevel DownlineLevel { get; }

    public bool IsViewingMemberList { get; }

    public bool ActorIsAgent { get; }

    public bool CanUpdateAccount { get; }

    public bool CanTransferOnBalanceColumn { get; }

    public bool CanTransferOnYesterdayBalanceColumn { get; }

    public bool CanViewBetLists { get; }

    public bool IsShowDateTimeColumn { get; }

    public string CookiePrefix { get; }

    public bool ShowLevelType { get; }

    public byte LevelType { get; }

    public string GetRunningBetListLink(CreditBalanceItem item)
        => Helpers.UrlHelper.MapBetListPath($"~/RunningBetList/Index?custId={item.CustId}&custName={item.UserName}&getAll=true");

    public string OldMainRootPath => MicroSites.SettingUrl();

    public string GivenCreditLink => MicroSites.AccountInfoUrl($"~/SearchAccount/GivenCredit");

    public string GetMemberBalanceInfoLink(string custName)
        => MicroSites.AccountInfoUrl($"~/SearchAccount/GetMemberBalanceInfo?custname={custName}");

    public string GetMemberTransferLink(int custId)
        => MicroSites.SettingUrl($"~/TransferYesterdayBalance/Member?custId={custId}");

    public EnteredSportResult EnteredSportResult { get; }

    private static string GetLegendViewByLanguage(string language)
    {
        const string viewName = "Legend";

        return language switch
        {
            "zh-TW" or "zh-CN" or "th-TH" or "vi-VN" or "ko-KR" => $"{viewName}.{language}",
            _ => $"{viewName}.en-US"
        };
    }
}