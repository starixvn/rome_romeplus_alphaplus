namespace AlphaPlus.Settings.Site.Features.MyWatchlist.MyWatchlistAccounts.ViewModels
{
    using AlphaPlus.Settings.Site.Features.MyWatchlist.MyWatchlistAccounts.Requests;
    using AlphaPlus.Settings.MemberInfo.Domain.Features.MyWatchlist.MyWatchlistAccounts.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.Settings.MemberInfo.Domain.Extensions;

    public class ManageWatchlistAccountsViewModel
    {
        public ManageWatchlistAccountsViewModel(
            ManageWatchlistAccountsQuery message, 
            IEnumerable<PersonalPresetModel> presets)
        {
            PresetSelectionViewModel = new PresetSelectionViewModel(
                presets.Select(preset => new SelectListItem
                {
                    Text = $"{preset.PresetTitle} ({preset.WinningThreshold.ToMoneyFormat()} - {preset.LosingThreshold.ToMoneyFormat()})",
                    Value = $"{preset.WinningThreshold.ToMoneyFormat()} - {preset.LosingThreshold.ToMoneyFormat()} - {preset.PresetTitle}",
                    Selected = false
                }),
                1);

            IsEditWatchlistAccountsPopup = message.IsEdit;
            Usernames = message.Usernames;
            if (IsEditWatchlistAccountsPopup)
            {
                CustIds = message.CustIds;
                Threshold = new ThresholdViewModel(message.WinningThreshold, message.LosingThreshold);
            }
        }

        public PresetSelectionViewModel PresetSelectionViewModel { get; }

        public bool IsEditWatchlistAccountsPopup { get; }

        public string CustIds { get; }

        public string Usernames { get; }

        public ThresholdViewModel Threshold { get; }
    }
}