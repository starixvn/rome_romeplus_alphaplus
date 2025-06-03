using AlphaPlus.AccountInfo.Site.Features.OpenDownlines.ViewModels;
using AlphaPlus.Core;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.OpenDownlines.Requests
{
    public class OpenSingleRequest : QueryBase, IRequest<OpenSingleViewModel>
    {
        public int SearchAccountId { get; set; }

        public string SearchAccountName { get; set; }
    }
}