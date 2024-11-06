using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Transfer.Models;
using AlphaPlus.Settings.Transfer.API.Models;
using MediatR;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.CashBalanceTransfer;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.CashBalanceTransfer
{
    public class CashBalanceTransferQuery : IRequest<CashBalanceTransferViewModel>
    {
        public CashBalanceTransferQuery() : this(null) { }

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