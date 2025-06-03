using AlphaPlus.Core;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.OnlineList
{
    public class OnlineListQuery : QueryBase, IRequest<IEnumerable<OnlineUser>>
    {
        public virtual int SearchAccountId { get; set; }

        public override bool IsValid() => SearchAccountId > 0 && !SearchAccountId.Equals(ViewerId);
    }
}