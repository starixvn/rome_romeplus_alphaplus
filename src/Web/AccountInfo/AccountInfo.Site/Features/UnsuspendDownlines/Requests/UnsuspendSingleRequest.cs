using AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.ViewModels;
using AlphaPlus.Core;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.Requests
{
    public class UnsuspendSingleRequest : QueryBase, IRequest<UnsuspendSingleViewModel>
    {
        public int SearchAccountId { get; set; }

        public string SearchAccountName { get; set; }
    }
}