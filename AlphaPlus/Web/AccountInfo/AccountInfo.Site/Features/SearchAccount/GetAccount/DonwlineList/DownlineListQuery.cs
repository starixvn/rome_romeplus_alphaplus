using AlphaPlus.AccountInfo.Site.Features.Shared;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.SearchAccount.GetAccount.DonwlineList
{
    public class DownlineListQuery : BaseQuery, IRequest<DownlineListViewModel>
    {
        public int PageIndex { get; set; }

        public byte RoleId { get; set; }

        public byte StatusId { get; set; }

        public int TotalPages { get; set; }
    }
}