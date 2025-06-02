using AlphaPlus.Core;
using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.Requests
{
    public class UnsuspendSingleCommand : QueryBase, IRequest<Result>
    {
        public int SearchAccountId { get; set; }

        public override bool IsValid() => base.IsValid() && CanUpdateAccount && !IsInternal;
    }
}