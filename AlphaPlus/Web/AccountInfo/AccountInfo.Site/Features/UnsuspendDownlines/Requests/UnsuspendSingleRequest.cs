using AlphaPlus.AccountInfo.Site.Features.Shared;
using AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.ViewModels;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.UnsuspendDownlines.Requests
{
    public class UnsuspendSingleRequest : BaseQuery, IRequest<UnsuspendSingleViewModel>
    {
        public string SearchAccountName { get; set; }
    }
}