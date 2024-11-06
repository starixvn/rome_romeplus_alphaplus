using AlphaPlus.Settings.MemberInfo.Domain.Extensions;
using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models;
using AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.Requests;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.MyWatchlist.MyWatchlistAccounts.ViewModels
{
    public class ManageWatchlistAccountsViewModel
    {
        public ManageWatchlistAccountsViewModel(
            ManageWatchlistAccountsQuery message,
            IEnumerable<PersonalPresetModel> presets,
            ThresholdViewModel threshold)
        {
            PresetSelectionViewModel = new PresetSelectionViewModel(
                presets.Select(preset => new SelectListItem
                {
                    Text = $"{preset.PresetTitle} ({preset.WinningThreshold.ToMoneyFormat()} - {preset.LosingThreshold.ToMoneyFormat()})",
                    Value = $"{preset.WinningThreshold.ToMoneyFormat()} - {preset.LosingThreshold.ToMoneyFormat()} - {preset.PresetTitle}",
                    Selected = false
                }), 1);
            Threshold = threshold;
            CustIds = message.CustIds;
            Usernames = message.Usernames;
            IsEditWatchlistAccountsPopup = message.IsEdit;
            MyWatchlistPage = message.MyWatchlistPage;
        }

        public PresetSelectionViewModel PresetSelectionViewModel { get; }

        public ThresholdViewModel Threshold { get; }

        public string CustIds { get; }

        public string Usernames { get; }

        public bool IsEditWatchlistAccountsPopup { get; }

        public bool MyWatchlistPage { get; }
    }
}