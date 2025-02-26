using MediatR;
using AlphaPlus.Features.Handbook.API.Models;

namespace AlphaPlus.Features.Site.Handbook.Requests
{
    public class SuggestArticleRequest : BaseHandbookRequest, IRequest<IEnumerable<SuggestionModel>>
    {
        public string Keyword { get; set; }
    }
}
