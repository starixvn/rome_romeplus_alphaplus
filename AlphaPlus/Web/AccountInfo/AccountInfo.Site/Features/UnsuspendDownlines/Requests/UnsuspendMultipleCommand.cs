using AlphaPlus.AccountInfo.Site.Features.Shared;
using AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.ViewModels;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.Requests
{
    public class UnsuspendCustomerInfo
    {
        public int SearchAccountId { get; set; }

        public string Username { get; set; }

        public bool Suspended { get; set; }
    }

    public class UnsuspendMultipleCommand : BaseQuery, IRequest<UnsuspendMultipleResultViewModel>
    {

        public IEnumerable<UnsuspendCustomerInfo> UnsuspendList { get; set; }

        public int RoleId { get; set; }

        public override bool IsValid() => CanUpdateAccount && !IsInternal;
    }
}