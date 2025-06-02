using AlphaPlus.AccountInfo.Site.Features.OpenDownlines.ViewModels;
using AlphaPlus.Core;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.OpenDownlines.Requests
{
    public class OpenMultipleRequest : QueryBase, IRequest<OpenMultipleViewModel>
    {
        public string CustIds { get; set; }

        public byte RoleId { get; set; }

        public override bool IsValid() => !string.IsNullOrEmpty(CustIds) && RoleId < userSession.RoleId;
    }
}