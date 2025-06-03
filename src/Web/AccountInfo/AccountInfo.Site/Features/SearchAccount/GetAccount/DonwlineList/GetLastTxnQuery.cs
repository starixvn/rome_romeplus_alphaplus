using AlphaPlus.Core;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.DonwlineList
{
    public class GetLastTxnQuery : QueryBase, IRequest<string>
    {
        public string Username { get; set; }

        public int UplineId => ViewerId;

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(Username) && UplineId > 0;
    }
}