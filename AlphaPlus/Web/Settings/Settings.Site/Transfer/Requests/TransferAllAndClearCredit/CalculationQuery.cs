namespace AlphaPlus.Settings.Site.Transfer.Requests.TransferAllAndClearCredit
{
    using MediatR;
    using Settings.Transfer.API.Models;
    using ViewModels.TransferAllAndClearCredit;

    public class CalculationQuery : TransferBaseQuery, IRequest<CalculationViewModel>
    {
        public CalculationQuery(IUser viewer) : base(viewer)
        {
            IsInternal = viewer.IsInternal;
            LanguageKey = viewer.LanguageKey;
        }

        
        public string Username { get; set; }

        public bool IsInternal { get; }

        public string AllowedTotalBalanceTransferSuperIds => Settings.Transfer.API.Configurations.AppSettings.AllowedTotalBalanceTransferSuperIds;

        public string LanguageKey { get; }
    }
}
