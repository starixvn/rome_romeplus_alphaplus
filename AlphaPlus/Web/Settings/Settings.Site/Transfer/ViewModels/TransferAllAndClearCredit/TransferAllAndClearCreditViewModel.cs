namespace AlphaPlus.Settings.Site.Transfer.ViewModels.TransferAllAndClearCredit
{
    using AlphaPlus.Core.Helpers;

    public static class TransferAllAndClearCreditViewModel
    {
        public static string GetOutstandingText(decimal? outstanding)
              => outstanding.HasValue ? outstanding.Value.ToString($"#,##0.00") : "N/A";

        public static string UsernameAutoSuggestionPath
            => MicroSites.SettingUrl("~/UserNameSuggestion");
    }
}