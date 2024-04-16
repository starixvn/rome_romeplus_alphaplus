namespace AlphaPlus.Settings.Site.Transfer.Requests.CashBalanceTransfer
{
    using AlphaPlus.Core.Models;
    using AlphaPlus.Settings.Site.Transfer.Models;
    using AlphaPlus.Settings.Transfer.API.Models;
    using MediatR;
    using ViewModels.CashBalanceTransfer;

    public class CashBalanceTransferQuery : IRequest<CashBalanceTransferViewModel>
    {
        public CashBalanceTransferQuery() : this(null) {}

        public CashBalanceTransferQuery(IUser user)
        {
            user = user ?? new User(UserSession.Current);
            UplineId = user.Id;
        }

        public int CustId { get; set; }

        public int UplineId { get; }

        public bool IsValid() => CustId > 0;
    }
}