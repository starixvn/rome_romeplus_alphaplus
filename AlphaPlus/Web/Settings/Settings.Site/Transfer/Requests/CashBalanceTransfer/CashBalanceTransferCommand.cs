namespace AlphaPlus.Settings.Site.Transfer.Requests.CashBalanceTransfer
{
    using AlphaPlus.Core.Models;
    using AlphaPlus.Settings.Site.Transfer.Models;
    using MediatR;
    using Settings.Transfer.API.Models;
    using Result = Settings.Transfer.API.Models.Result;

    public class CashBalanceTransferCommand : IRequest<Result>
    {
        public CashBalanceTransferCommand() : this(null) {}

        public CashBalanceTransferCommand(IUser user)
        {
            user = user ?? new User(UserSession.Current);
            UplineId = user.Id;
            SubAccId = user.SubAccId;
            IP = user.IP;
            LanguageKey = user.LanguageKey;
            UplineName = user.SubAccId > 0 ? user.SubAccName : user.Name;
            AGIP = user.AGIP;
        }

        public int CustId { get; set; }

        public decimal Amount { get; set; }

        public int UplineId { get; }

        public string UplineName { get; }

        public int SubAccId { get; }

        public string IP { get; }

        public string LanguageKey { get; }

        public string AGIP { get; }

        public bool IsValid() => CustId > 0 && Amount != 0 && UplineId > 0;
    }
}