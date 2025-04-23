using AlphaPlus.AccountInfo.Site.Features.Shared;
using AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.ViewModels;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.Requests
{
    public class SuspendMultipleConfirmRequest : BaseQuery, IRequest<SuspendMultipleConfirmViewModel>
    {
        public string UserNames { get; set; }

        public string Ids { get; set; }

        public byte RoleId { get; set; }

        public override bool IsValid() => !string.IsNullOrEmpty(UserNames) && !string.IsNullOrEmpty(Ids);
    }
}