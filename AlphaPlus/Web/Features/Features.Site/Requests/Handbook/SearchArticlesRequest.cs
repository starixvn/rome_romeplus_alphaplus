using MediatR;
using AlphaPlus.Features.Site.Handbook.ViewModels;

namespace AlphaPlus.Features.Site.Handbook.Requests
{
    public class SearchArticlesRequest : BaseHandbookRequest, IRequest<SearchArticleViewModel>
    {
        public string Keyword { get; set; }
    }
}
