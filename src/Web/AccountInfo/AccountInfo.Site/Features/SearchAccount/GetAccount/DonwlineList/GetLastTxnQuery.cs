using AlphaPlus.AccountInfo.Site.Features.Shared;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.DonwlineList
{
    public class GetLastTxnQuery : BaseQuery, IRequest<string>
    {
        public string Username { get; set; }

        public int UplineId => ViewerId;

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Username) && UplineId > 0;
    }
}