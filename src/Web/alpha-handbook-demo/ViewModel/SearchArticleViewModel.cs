using Alpha.Handbook.Demo.Constants;
using Alpha.Handbook.Demo.Models;

namespace Alpha.Handbook.Demo.ViewModels
{
    public class SearchArticleViewModel
    {
        public IEnumerable<ArticleModel> Articles { get; set; }
        public string Language { get; set; }
        public string Locale => LanguageConsts.Locales[Language];
    }
}