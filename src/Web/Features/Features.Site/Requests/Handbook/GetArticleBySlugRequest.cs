using MediatR;
using AlphaPlus.Features.Site.Handbook.ViewModels;

namespace AlphaPlus.Features.Site.Handbook.Requests
{
    public class GetArticleBySlugRequest : BaseHandbookRequest, IRequest<ArticleViewModel>
    {
        public string Slug { get; set; }
    }
}
