using AlphaPlus.AccountInfo.Site.Features.Shared;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.DonwlineList
{
    public class TotalDownlineQuery : BaseQuery, IRequest<IEnumerable<TotalDownlinesSummary>>
    {
        public byte RoleId { get; set; }
    }
}