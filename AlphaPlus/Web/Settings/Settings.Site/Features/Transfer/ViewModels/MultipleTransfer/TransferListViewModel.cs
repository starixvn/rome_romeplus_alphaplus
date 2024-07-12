using AlphaPlus.Shared.AppResources.Settings.Transfer;
using AlphaPlus.Settings.Site.Features.Transfer.Requests.MultipleTransfer;
using AlphaPlus.Settings.Transfer.API.Models.MultipleTransfer;
using Microsoft.AspNetCore.Mvc.Rendering;
using AlphaPlus.Settings.MemberInfo.Domain.Models.Winloss;

namespace AlphaPlus.Settings.Site.Features.Transfer.ViewModels.MultipleTransfer;

public class TransferListViewModel(
    TransferListQuery message,
    EnteredSportResult enteredSportResult,
    CustomerTransferReport reportData,
    bool allowAllDayBalanceTransfer)
{
    public string Username { get; } = message.Username;

    public string Status { get; } = message.Status;

    public IEnumerable<SelectListItem> StatusOptions { get; } =
    [
        new SelectListItem(Resources.All, "all"),
        new SelectListItem(Resources.Open, "open"),
        new SelectListItem(Resources.Suspended, "suspended"),
        new SelectListItem(Resources.Closed, "closed"),
        new SelectListItem(Resources.Disabled, "disabled")
    ];

    public byte AutoTransfer { get; } = message.AutoTransfer;

    public IEnumerable<SelectListItem> AutoTransferOptions { get; } =
    [
        new SelectListItem(Resources.All, "2"),
        new SelectListItem(Resources.Enabled, "1"),
        new SelectListItem(Resources.Disabled, "0")
    ];

    public bool AllYesterdayBalance { get; } = message.AllYesterdayBalance;

    public bool AllowTotalBalanceTransfer { get; } = message.AllowTotalBalanceTransfer;

    public bool AllowAllDayBalanceTransfer { get; } = allowAllDayBalanceTransfer;

    public int PageIndex { get; } = message.PageIndex;

    public int PageSize { get; } = message.PageSize;

    public EnteredSportResult EnteredSportResult { get; } = enteredSportResult;

    public CustomerTransferReport ReportData { get; } = reportData;

    // public string UsernameAutoSuggestionPath
    //     => MicroSites.SettingUrl("~/UserNameSuggestion");

    // public string TransferInformationPath
    //     => MicroSites.SettingUrl("~/cashbalancetransfer/hint");

    public bool CanTransferYesterday(CustomerTransfer customer)
    {
        if (AllowAllDayBalanceTransfer)
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
        if (AllowAllDayBalanceTransfer)
        {
            if (AllowTotalBalanceTransfer && (customer.Balance == 0 || customer.YesterdayBalance == 0))
            {
                return string.Empty;
            }

            if (customer.YesterdayBalance < 0)
            {
                return "favorite";
            }
        }

        if (AllowTotalBalanceTransfer)
        {
            return string.Empty;
        }

        return customer.YesterdayBalance < 0 ? "favorite" : "underdog";
    }

    public string GetBalanceColor(CustomerTransfer customer)
        => customer.Balance < 0 ? "favorite" : string.Empty;

    public string GetCashBalanceColor(decimal cashBalance) => cashBalance < 0 ? "favorite" : string.Empty;

    public bool ShowYesterdayBalanceLink(CustomerTransfer customer)
        => !AllowAllDayBalanceTransfer && !AllowTotalBalanceTransfer && customer.YesterdayBalance != 0;

    public bool ShowBalanceLink(CustomerTransfer customer)
        => !AllowAllDayBalanceTransfer && AllowTotalBalanceTransfer && customer.Balance != 0;

    public bool ShowCashBalanceLink() => AllowAllDayBalanceTransfer;
}
