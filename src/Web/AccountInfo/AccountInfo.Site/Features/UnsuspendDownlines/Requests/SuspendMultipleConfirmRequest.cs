using AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.ViewModels;
using AlphaPlus.Core;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.Requests
{
    public class SuspendMultipleConfirmRequest : QueryBase, IRequest<SuspendMultipleConfirmViewModel>
    {
        public string UserNames { get; set; }

        public string Ids { get; set; }

        public byte RoleId { get; set; }

        public override bool IsValid() => !string.IsNullOrEmpty(UserNames) && !string.IsNullOrEmpty(Ids);
    }
}