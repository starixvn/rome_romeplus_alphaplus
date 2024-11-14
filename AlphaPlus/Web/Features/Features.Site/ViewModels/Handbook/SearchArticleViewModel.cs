using AlphaPlus.Features.Handbook.API.Constants;
using AlphaPlus.Features.Handbook.API.Models;

namespace AlphaPlus.Features.Site.Handbook.ViewModels
{
    public class SearchArticleViewModel
    {
        public IEnumerable<ArticleModel> Articles { get; set; }
        public string Language { get; set; }
        public string Locale => LanguageConsts.Locales[Language];
    }
}