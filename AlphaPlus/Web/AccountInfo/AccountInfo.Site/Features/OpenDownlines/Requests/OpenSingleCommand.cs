using AlphaPlus.AccountInfo.Site.Features.Shared;
using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.OpenDownlines.Requests
{
    public class OpenSingleCommand : BaseQuery, IRequest<Result>
    {
        public override bool IsValid() => base.IsValid() && CanUpdateAccount && !IsInternal;
    }
}