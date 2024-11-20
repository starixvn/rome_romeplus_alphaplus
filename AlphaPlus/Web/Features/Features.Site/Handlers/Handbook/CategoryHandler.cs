using AlphaPlus.Features.Handbook.API.Constants;
using AlphaPlus.Features.Handbook.API.Helpers;
using AlphaPlus.Features.Handbook.API.Models;
using AlphaPlus.Features.Site.Handbook.Requests;
using AlphaPlus.Features.Site.Handbook.ViewModels;
using MediatR;

namespace AlphaPlus.Features.Site.Handbook.Handlers
{
    public class CategoryHandler : 
        APIHandler,
        IRequestHandler<GetAllCategoriesRequest, IEnumerable<CategoryModel>>,
        IRequestHandler<GetCategoryBySlugRequest, CategoryViewModel>
    {
        public CategoryHandler(Fanex.Logging.ILogger logger) : base(logger) { }

        public Task<IEnumerable<CategoryModel>> Handle(GetAllCategoriesRequest request, CancellationToken cancellation)
        {
            var langKey = request.LanguageKey.ToLowerInvariant();
            var url = contentAPIUrl + "/api/categories/latest/all" +
                "/" + LanguageConsts.Locales[langKey] +
                "/" + AppSettingsHelper.SiteLabel +
                "/" + request.RoleId +
                "/" + AppSettingsHelper.ContentAPIEnvironment;
            var categories = Get<IEnumerable<CategoryModel>>(url) ?? new List<CategoryModel>();
            return Task.FromResult(categories);
        }

        public Task<CategoryViewModel> Handle(GetCategoryBySlugRequest request, CancellationToken cancellation)
        {
            var langKey = request.LanguageKey.ToLowerInvariant();
            var url = contentAPIUrl + "/api/categories/latest" +
                "/" + request.Slug +
                "/" + LanguageConsts.Locales[langKey] +
                "/" + AppSettingsHelper.SiteLabel +
                "/" + request.RoleId +
                "/" + AppSettingsHelper.ContentAPIEnvironment;
            var category = Get<CategoryModel>(url) ?? new CategoryModel();
            category.Articles = category.Articles.OrderBy(a => a.Order);
            var locale = LanguageConsts.Locales[langKey];
            var viewModel = new CategoryViewModel
            {
                Language = locale,
                Category = category
            };

            return Task.FromResult(viewModel);
        }
    }
}