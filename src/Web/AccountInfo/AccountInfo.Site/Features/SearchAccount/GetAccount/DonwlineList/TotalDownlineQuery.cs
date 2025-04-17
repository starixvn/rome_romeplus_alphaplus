using AlphaPlus.Core;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.DonwlineList
{
    public class TotalDownlineQuery : QueryBase, IRequest<IEnumerable<TotalDownlinesSummary>>
    {
        public byte RoleId { get; set; }
    }
}