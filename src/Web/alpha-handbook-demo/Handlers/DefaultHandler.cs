using Alpha.Handbook.Demo.Models;
using Alpha.Handbook.Demo.Requests;
using Alpha.Handbook.Demo.ViewModels;
using MediatR;

namespace Alpha.Handbook.Demo.Handlers
{
    public class DefaultHandler : 
        APIHandler,
        IRequestHandler<HandbookGetBySlugRequest, GetBySlugViewModel>
    {
        public DefaultHandler(Fanex.Logging.ILogger logger) : base(logger)
        {
        }

        public Task<GetBySlugViewModel> Handle(HandbookGetBySlugRequest request, CancellationToken cancellation)
        {
            var briefProfile = new BriefProfileViewModel();
            var langKey = request.LanguageKey.ToLowerInvariant();

             var url = contentAPIUrl + "/api/categories/latest/all" +
                "/" + request.Language +
                "/" + request.SiteLabel +
                "/" + request.RoleId +
                "/" + request.Environment;
            var categories = Get<IEnumerable<CategoryModel>>(url) ?? new List<CategoryModel>();
            foreach (var c in categories)
            {
                if (c.Articles != null)
                {
                    c.Articles = c.Articles.OrderBy(a => a.Order);
                }
            }

            var viewModel = new GetBySlugViewModel
            {
                LanguageKey = langKey,
                BriefProfile = briefProfile,
                Categories = categories.OrderBy(c => c.Order),
                Slug = request.Slug,
                Id = request.Id
            };

            return Task.FromResult(viewModel);
        }
    }
}