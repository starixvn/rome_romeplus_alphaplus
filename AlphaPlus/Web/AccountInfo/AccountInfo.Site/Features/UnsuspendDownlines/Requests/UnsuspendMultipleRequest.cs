using AlphaPlus.AccountInfo.Site.Features.Shared;
using AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.ViewModels;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.Requests
{
    public class UnsuspendMultipleRequest : BaseQuery, IRequest<UnsuspendMultipleViewModel>
    {
        public string CustIds { get; set; }

        public byte RoleId { get; set; }


        public override bool IsValid() => !string.IsNullOrEmpty(CustIds) && RoleId < ViewerId;
    }
}