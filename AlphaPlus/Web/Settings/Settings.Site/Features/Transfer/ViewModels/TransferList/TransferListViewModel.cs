using AlphaPlus.Shared.AppResources.Settings.Transfer;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.TransferList;
using AlphaPlus.Settings.Transfer.API.Models.TransferList;
using Microsoft.AspNetCore.Mvc.Rendering;
using AlphaPlus.Settings.MemberInfo.Domain.Models.Winloss;
using AlphaPlus.Settings.Site.Features.Transfer.Constants;
using AlphaPlus.Core.Helpers;

namespace AlphaPlus.Settings.Site.Features.Transfer.ViewModels.TransferList;

public class TransferListViewModel(
    TransferListQuery message,
    EnteredSportResult enteredSportResult,
    CustomerTransferReport reportData,
    bool isAllDayBalanceTransfer,
    bool isEnableAutoTransfer,
    string transferOption,
    string transferCondition)
{
    public int ViewerId { get; } = message.ViewerId;

    public string Username { get; } = message.Username;

    public string Status { get; } = message.Status;

    public IEnumerable<SelectListItem> StatusOptions { get; } =
    [
        new SelectListItem(Resources.All, StatusConstant.All),
        new SelectListItem(Resources.Open, StatusConstant.Open),
        new SelectListItem(Resources.Suspended, StatusConstant.Suspended),
        new SelectListItem(Resources.Closed, StatusConstant.Closed),
        new SelectListItem(Resources.Disabled, StatusConstant.Disabled)
    ];

    public byte AutoTransfer { get; } = message.AutoTransfer;

    public IEnumerable<SelectListItem> AutoTransferOptions { get; } =
    [
        new SelectListItem(Resources.All, AutoTransferConstant.All.ToString()),
        new SelectListItem(Resources.Enabled, AutoTransferConstant.Enabled.ToString()),
        new SelectListItem(Resources.Disabled, AutoTransferConstant.Disabled.ToString())
    ];

    public bool AllYesterdayBalance { get; } = message.AllYesterdayBalance;

    public bool AllowTotalBalanceTransfer { get; } = message.AllowTotalBalanceTransfer;

    public bool IsAllDayBalanceTransfer { get; } = isAllDayBalanceTransfer;

    public bool IsEnableAutoTransfer { get; } = isEnableAutoTransfer;

    public int PageIndex { get; } = message.PageIndex;

    public int PageSize { get; } = message.PageSize;

    public IEnumerable<SelectListItem> PageSizes { get; } =
        new int[] { 10, 50, 100, 500 }
        .Select(p => new SelectListItem(p.ToString(), p.ToString(), p == message.PageSize))
        .ToList();

    public EnteredSportResult EnteredSportResult { get; } = enteredSportResult;

    public CustomerTransferReport ReportData { get; } = reportData;

    public bool CanTransferYesterday(CustomerTransfer customer)
    {
        if (IsAllDayBalanceTransfer)
        {
            return AllowTotalBalanceTransfer && customer.Balance != 0 && customer.YesterdayBalance != 0;
        }

        if (AllowTotalBalanceTransfer)
        {
            return customer.Balance != 0;
        }

        return customer.YesterdayBalance != 0;
    }

    public string GetYesterdayBalanceColor(CustomerTransfer customer)
    {
        if (IsAllDayBalanceTransfer)
        {
            return !(AllowTotalBalanceTransfer && customer.Balance == 0) && customer.YesterdayBalance < 0
                ? "favorite"
                : string.Empty;
        }

        if (AllowTotalBalanceTransfer)
        {
            return string.Empty;
        }

        return customer.YesterdayBalance < 0 ? "favorite" : "underdog";
    }

    public string GetCashBalanceColor(decimal cashBalance) => cashBalance < 0 ? "favorite" : string.Empty;

    public bool ShowYesterdayBalanceLink(CustomerTransfer customer)
        => !IsAllDayBalanceTransfer && !AllowTotalBalanceTransfer && customer.YesterdayBalance != 0;

    public bool ShowBalanceLink(CustomerTransfer customer)
        => !IsAllDayBalanceTransfer && AllowTotalBalanceTransfer && customer.Balance != 0;

    public IDictionary<string, string> GetAutoTransferParams(CustomerTransfer customer)
        => new Dictionary<string, string>
        {
            [nameof(customer.CustId)] = customer.CustId.ToString(),
            ["CustName"] = customer.Username,
            ["State"] = customer.IsAllowAutoTransfer ? "off" : "on",
            ["Old_TransferOption"] = transferOption
        };

    public IDictionary<string, string> GetScheduleTransferParams(CustomerTransfer customer)
        => new Dictionary<string, string>
        {
            [nameof(customer.CustId)] = customer.CustId.ToString(),
            ["CustName"] = customer.Username,
            ["Schedule"] = customer.ScheduleWeekDays,
            ["Old_TransferOption"] = transferOption,
            ["UplineCondition"] = transferCondition
        };

    public IDictionary<string, string> GetSingleTransferParams(CustomerTransfer customer)
        => new Dictionary<string, string>
        {
            [nameof(customer.CustId)] = customer.CustId.ToString(),
            ["CustName"] = customer.Username,
            ["ShowTitle"] = "1"
        };

    public string UpdateMultipleAutoTransferUrl
        => MicroSites.SettingUrl($"~/AutoTransfer/UpdateMultipleAutoTransfer?uplineCondition={transferCondition}&Old_TransferOption={transferOption}");
}
