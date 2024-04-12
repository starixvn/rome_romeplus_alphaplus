namespace AlphaPlus.Settings.Site.Transfer.Requests.CashBalanceTransfer
{
    using AlphaPlus.Settings.Transfer.API.Models;
    using MediatR;
    using ViewModels.CashBalanceTransfer;

    public class CashBalanceTransferQuery : IRequest<CashBalanceTransferViewModel>
    {
        public CashBalanceTransferQuery(IUser user)
        {
            UplineId = user.Id;
        }

        public int CustId { get; set; }

        public int UplineId { get; }

        public bool IsValid() => CustId > 0;
    }
}