using AlphaPlus.Core;
using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.OpenDownlines.Requests
{
    public class OpenSingleCommand : QueryBase, IRequest<Result>
    {
        public int SearchAccountId { get; set; }

        public override bool IsValid() => base.IsValid() && CanUpdateAccount && !IsInternal;
    }
}