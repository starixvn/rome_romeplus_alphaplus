using Alpha.Handbook.Demo.Models;
using Alpha.Handbook.Demo.Requests;
using Alpha.Handbook.Demo.ViewModels;
using MediatR;

namespace Alpha.Handbook.Demo.Handlers
{
    public class ArticleHandler : 
        APIHandler, 
        IRequestHandler<SearchArticlesRequest, SearchArticleViewModel>,
        IRequestHandler<GetArticleBySlugRequest, ArticleViewModel>,
        IRequestHandler<SuggestArticleRequest, IEnumerable<SuggestionModel>>,
        IRequestHandler<GetArticleByIdRequest, ArticleViewModel>
    {
        private const int SuggestedLimit = 10;

        public ArticleHandler(Fanex.Logging.ILogger logger) : base(logger)
        {
        }

        public Task<SearchArticleViewModel> Handle(SearchArticlesRequest request, CancellationToken cancellation)
        {
            var langKey = request.LanguageKey.ToLowerInvariant();
            var url = contentAPIUrl + "/api/articles/search" +
                "/" + request.Language +
                "/" + request.SiteLabel +
                "/" + request.RoleId +
                "/" + request.Environment +
                "/k" + request.Keyword;

            var articles = Get<IEnumerable<ArticleModel>>(url) ?? Enumerable.Empty<ArticleModel>();

            foreach (var a in articles)
            {
                a.CorrectImageUrls();
            }

            var viewModel = new SearchArticleViewModel
            {
                Language = langKey,
                Articles = articles.OrderBy(a => a.Title)
            };

            return Task.FromResult(viewModel);
        }

        public Task<ArticleViewModel> Handle(GetArticleBySlugRequest request, CancellationToken cancellation)
        {
            var langKey = request.LanguageKey.ToLowerInvariant();
            var url = contentAPIUrl + "/api/articles/get/" + request.Slug +
                "/" + request.Language +
                "/" + request.SiteLabel +
                "/" + request.RoleId +
                "/" + request.Environment;
            var article = Get<ArticleModel>(url);
            if (article != null)
            {
                article.CorrectImageUrls();
            }

            var viewModel = new ArticleViewModel
            {
                Article = article,
                Language = request.Language
            };

            return Task.FromResult(viewModel);
        }

        public Task<IEnumerable<SuggestionModel>> Handle(SuggestArticleRequest request, CancellationToken cancellation)
        {
            var langKey = request.LanguageKey.ToLowerInvariant();
            var url = contentAPIUrl + "/api/articles/suggest" +
                "/" + request.Language +
                "/" + request.SiteLabel +
                "/" + request.RoleId +
                "/" + request.Environment +
                "/" + request.Keyword;

            var articles = Get<IEnumerable<SuggestionModel>>(url) ?? new List<SuggestionModel>();

            return Task.FromResult<IEnumerable<SuggestionModel>>(articles.Take(SuggestedLimit).OrderBy(r => r.Value));
        }

        public Task<ArticleViewModel> Handle(GetArticleByIdRequest request, CancellationToken cancellation)
        {
            var langKey = request.LanguageKey.ToLowerInvariant();
            var url = contentAPIUrl + "/api/articles/getById/" + request.Id +
                "/" + request.Language +
                "/" + request.SiteLabel +
                "/" + request.RoleId +
                "/" + request.Environment;
            var article = Get<ArticleModel>(url);
            if (article != null)
            {
                article.CorrectImageUrls();
            }

            var viewModel = new ArticleViewModel
            {
                Article = article,
                Language = request.Language
            };

            return Task.FromResult(viewModel);
        }
    }
}