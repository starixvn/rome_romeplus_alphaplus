using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.Transfer.Models;
using MediatR;
using AlphaPlus.Settings.Transfer.API.Models;
using AlphaPlus.Settings.Site.Features.Transfer.ViewModels.CashBalanceTransfer;

namespace AlphaPlus.Settings.Site.Features.Transfer.Requests.CashBalanceTransfer
{
    public class HintTransferQuery : IRequest<HintTransferViewModel>
    {
        public HintTransferQuery() : this(null) { }

        public HintTransferQuery(IUser user)
        {
            user = user ?? new User(UserSession.Current);
            CustId = user.Id;
            RoleId = user.LevelId;
            Language = user.Language;
            Currency = user.Currency;
            User = user;
        }

        public IUser User { get; }

        public int CustId { get; }

        public byte RoleId { get; }

        public string Language { get; }

        public string Currency { get; }

        public bool IsValid() => CustId > 0;
    }
}