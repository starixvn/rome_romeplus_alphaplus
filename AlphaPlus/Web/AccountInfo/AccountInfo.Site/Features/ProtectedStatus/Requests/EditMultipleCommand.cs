using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaPlus.AccountInfo.Site.Features.Shared;
using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.ProtectedStatus.Requests
{
    public class EditMultipleCommand : BaseQuery, IRequest<IEnumerable<Result>>
    {
        public CustomerProtectedStatus[] ProtectedStatus { get; set; }

        public override bool IsValid() => CanUpdateAccount && !IsInternal && ProtectedStatus?.Length > 0;
    }
}