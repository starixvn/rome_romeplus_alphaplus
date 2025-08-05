using MediatR;
using Alpha.Handbook.Demo.Models;

namespace Alpha.Handbook.Demo.Requests
{
    public class SuggestArticleRequest : BaseHandbookRequest, IRequest<IEnumerable<SuggestionModel>>
    {
        public string Keyword { get; set; }
    }
}
