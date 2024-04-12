namespace AlphaPlus.Settings.Site.Transfer.Requests.TransferAllAndClearCredit
{
    using MediatR;
    using Settings.Transfer.API.Models;
    using ViewModels.TransferAllAndClearCredit;

    public class TransferResultQuery : TransferBaseQuery, IRequest<TransferResultViewModel>
    {
        public TransferResultQuery(IUser viewer) : base(viewer)
        {
            UserId = viewer.UserId;
            SubAccId = viewer.SubAccId;
            IP = viewer.IP;
            LanguageKey = viewer.LanguageKey;
            IsInternal = viewer.IsInternal;
        }

        public int UserId { get; }

        public int SubAccId { get; }

        public bool IsClearCredit { get; set; }

        
        public string Username { get; set; }

        public string AllowedTotalBalanceTransferSuperIds => Settings.Transfer.API.Configurations.AppSettings.AllowedTotalBalanceTransferSuperIds;

        public string IP { get; set; }

        public string TransferSteps { get; set; }

        public string ClearCreditSteps { get; set; }

        public string LanguageKey { get; set; }

        public bool IsInternal { get; }
    }
}