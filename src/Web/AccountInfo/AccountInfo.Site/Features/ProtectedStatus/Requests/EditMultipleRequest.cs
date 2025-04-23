using AlphaPlus.AccountInfo.Site.Features.Shared;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.ProtectedStatus.Requests
{
    public class EditMultipleRequest : BaseQuery, IRequest<EditMultipleViewModel>
    {
        public string CustIds { get; set; }
    }
}