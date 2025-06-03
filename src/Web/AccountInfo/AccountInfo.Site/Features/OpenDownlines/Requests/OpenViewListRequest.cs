using AlphaPlus.AccountInfo.Site.Features.OpenDownlines.ViewModels;
using AlphaPlus.Core;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.OpenDownlines.Requests
{
    public class OpenViewListRequest : QueryBase, IRequest<OpenListViewModel>
    {
        public int SearchAccountId { get; set; }

        public string SearchAccountName { get; set; }

        public bool IsAffected { get; set; }

        public byte DownlineRoleId { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public override bool IsValid() => base.IsValid() && DownlineRoleId < userSession.RoleId;
    }
}