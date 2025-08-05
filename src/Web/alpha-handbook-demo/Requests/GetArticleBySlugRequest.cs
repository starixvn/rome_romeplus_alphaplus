using Alpha.Handbook.Demo.ViewModels;
using MediatR;

namespace Alpha.Handbook.Demo.Requests
{
    public class GetArticleBySlugRequest : BaseHandbookRequest, IRequest<ArticleViewModel>
    {
        public string Slug { get; set; }
    }
}