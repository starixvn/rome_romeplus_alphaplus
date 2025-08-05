using MediatR;
using Alpha.Handbook.Demo.ViewModels;

namespace Alpha.Handbook.Demo.Requests
{
    public class SearchArticlesRequest : BaseHandbookRequest, IRequest<SearchArticleViewModel>
    {
        public string Keyword { get; set; }
    }
}
