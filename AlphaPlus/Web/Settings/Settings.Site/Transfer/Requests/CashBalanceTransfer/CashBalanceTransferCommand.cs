namespace AlphaPlus.Settings.Site.Transfer.Requests.CashBalanceTransfer
{
    using MediatR;
    using Settings.Transfer.API.Models;

    public class CashBalanceTransferCommand : IRequest<Result>
    {
        public CashBalanceTransferCommand(IUser user)
        {
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