using AlphaPlus.Core;
using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.ProtectedStatus.Requests
{
    public class EditMultipleCommand : QueryBase, IRequest<IEnumerable<Result>>
    {
        public CustomerProtectedStatus[] ProtectedStatus { get; set; }

        public override bool IsValid() => CanUpdateAccount && !IsInternal && ProtectedStatus?.Length > 0;
    }
}