using Alpha.Handbook.Demo.Models;
using Alpha.Handbook.Demo.Requests;
using Alpha.Handbook.Demo.ViewModels;
using MediatR;

namespace Alpha.Handbook.Demo.Handlers
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
                "/" + request.Language +
                "/" + request.SiteLabel +
                "/" + request.RoleId +
                "/" + request.Environment;
            var categories = Get<IEnumerable<CategoryModel>>(url) ?? new List<CategoryModel>();
            return Task.FromResult(categories);
        }

        public Task<CategoryViewModel> Handle(GetCategoryBySlugRequest request, CancellationToken cancellation)
        {
            var langKey = request.LanguageKey.ToLowerInvariant();
            var url = contentAPIUrl + "/api/categories/latest" +
                "/" + request.Slug +
                "/" + request.Language +
                "/" + request.SiteLabel +
                "/" + request.RoleId +
                "/" + request.Environment;
            var category = Get<CategoryModel>(url) ?? new CategoryModel();
            category.Articles = category.Articles.OrderBy(a => a.Order);
            var viewModel = new CategoryViewModel
            {
                Language = request.Language,
                Category = category
            };

            return Task.FromResult(viewModel);
        }
    }
}