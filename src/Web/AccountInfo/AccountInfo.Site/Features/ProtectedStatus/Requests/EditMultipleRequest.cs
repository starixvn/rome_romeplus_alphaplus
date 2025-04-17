using AlphaPlus.Core;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.ProtectedStatus.Requests
{
    public class EditMultipleRequest : QueryBase, IRequest<EditMultipleViewModel>
    {
        public string CustIds { get; set; }
    }
}