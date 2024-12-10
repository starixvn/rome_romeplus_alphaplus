using AlphaPlus.AccountInfo.Site.Features.OpenDownlines.ViewModels;
using AlphaPlus.AccountInfo.Site.Features.Shared;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.OpenDownlines.Requests
{
    public class OpenSingleRequest : BaseQuery, IRequest<OpenSingleViewModel>
    {
        public string SearchAccountName { get; set; }
    }
}