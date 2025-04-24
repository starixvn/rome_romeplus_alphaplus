using AlphaPlus.AccountInfo.Site.Features.Shared;
using MediatR;
using AlphaPlus.AccountInfo.Site.Features.OpenDownlines.ViewModels;

namespace AlphaPlus.AccountInfo.Site.Features.OpenDownlines.Requests
{
    public class OpenMultipleRequest : BaseQuery, IRequest<OpenMultipleViewModel>
    {
        public string CustIds { get; set; }

        public byte RoleId { get; set; }

        public override bool IsValid() => !string.IsNullOrEmpty(CustIds) && RoleId < userSession.RoleId;
    }
}