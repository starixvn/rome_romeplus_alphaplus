using AlphaPlus.AccountInfo.Site.Features.OpenDownlines.ViewModels;
using AlphaPlus.Core;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.OpenDownlines.Requests
{
    public class OpenCustomerInfo
    {
        public int SearchAccountId { get; set; }

        public string Username { get; set; }

        public bool IsClosed { get; set; }
    }

    public class OpenMultipleCommand : QueryBase, IRequest<OpenMultipleResultViewModel>
    {
        public IEnumerable<OpenCustomerInfo> OpenList { get; set; }

        public int RoleId { get; set; }

        public override bool IsValid() => CanUpdateAccount && !IsInternal;
    }
}