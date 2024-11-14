using AlphaPlus.Features.Handbook.API.Constants;
using AlphaPlus.Features.Handbook.API.Helpers;
using AlphaPlus.Features.Handbook.API.Models;
using AlphaPlus.Features.Site.Handbook.Requests;
using AlphaPlus.Features.Site.Handbook.ViewModels;
using MediatR;

namespace AlphaPlus.Features.Site.Handbook.Handlers
{
    public class ArticleHandler : 
        APIHandler,
        IRequestHandler<SearchArticlesRequest, SearchArticleViewModel>,
        IRequestHandler<GetArticleBySlugRequest, ArticleViewModel>,
        IRequestHandler<SuggestArticleRequest, IEnumerable<SuggestionModel>>
    {
        private const int SuggestedLimit = 10;

        public ArticleHandler(Fanex.Logging.ILogger logger) : base(logger) { }

        public async Task<SearchArticleViewModel> Handle(SearchArticlesRequest request, CancellationToken cancellation)
        {
            var langKey = request.LanguageKey.ToLowerInvariant();
            var url = contentAPIUrl + "/api/articles/search" +
                "/" + LanguageConsts.Locales[langKey] +
                "/" + AppSettingsHelper.SiteLabel +
                "/" + request.RoleId +
                "/" + AppSettingsHelper.ContentAPIEnvironment +
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

            return viewModel;
        }

        public async Task<ArticleViewModel> Handle(GetArticleBySlugRequest request, CancellationToken cancellation)
        {
            var langKey = request.LanguageKey.ToLowerInvariant();
            var url = contentAPIUrl + "/api/articles/get/" + request.Slug +
                "/" + LanguageConsts.Locales[langKey] +
                "/" + AppSettingsHelper.SiteLabel +
                "/" + request.RoleId +
                "/" + AppSettingsHelper.ContentAPIEnvironment;
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

            return viewModel;
        }

        public async Task<IEnumerable<SuggestionModel>> Handle(SuggestArticleRequest request, CancellationToken cancellation)
        {
            var langKey = request.LanguageKey.ToLowerInvariant();
            var url = contentAPIUrl + "/api/articles/suggest" +
                "/" + LanguageConsts.Locales[langKey] +
                "/" + AppSettingsHelper.SiteLabel +
                "/" + request.RoleId +
                "/" + AppSettingsHelper.ContentAPIEnvironment +
                "/" + request.Keyword;

            var articles = Get<IEnumerable<SuggestionModel>>(url) ?? new List<SuggestionModel>();

            return articles.Take(SuggestedLimit).OrderBy(r => r.Value);
        }
    }
}