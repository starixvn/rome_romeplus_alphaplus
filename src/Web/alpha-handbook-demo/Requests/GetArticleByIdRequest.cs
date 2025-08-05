using Alpha.Handbook.Demo.ViewModels;
using MediatR;

namespace Alpha.Handbook.Demo.Requests
{
    public class GetArticleByIdRequest : BaseHandbookRequest, IRequest<ArticleViewModel>
    {
        public string Id { get; set; }
    }
}