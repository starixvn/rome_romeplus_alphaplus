using System.Collections.Generic;
using AlphaPlus.Core.Helpers;
using AlphaPlus.Features.Handbook.API.Constants;

namespace AlphaPlus.Features.Handbook.API.Models
{
    public class CategoryModel
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public string ShortName { get; set; }
        public int? Order { get; set; }
        public IEnumerable<CategoryModel> CategoriesParent { get; set; } = new List<CategoryModel>();
        public IEnumerable<CategoryModel> Categories { get; set; } = new List<CategoryModel>();
        public IEnumerable<MarketModel> Markets { get; set; } = new List<MarketModel>();
        public IEnumerable<ArticleModel> Articles { get; set; } = new List<ArticleModel>();

        public string GetUrl(string relativePath = "")
            => $"{relativePath}HandbookCategory/GetBySlug?slug=" + Slug;
    }
}