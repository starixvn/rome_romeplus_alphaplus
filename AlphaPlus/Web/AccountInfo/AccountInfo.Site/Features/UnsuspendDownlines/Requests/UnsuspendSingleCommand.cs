using AlphaPlus.AccountInfo.Site.Features.Shared;
using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.Requests
{
    public class UnsuspendSingleCommand : BaseQuery, IRequest<Result>
    {
        public int SearchAccountId { get; set; }

        public override bool IsValid() => base.IsValid() && CanUpdateAccount && !IsInternal;
    }
}