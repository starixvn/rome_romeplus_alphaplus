using AlphaPlus.Features.Handbook.API.Constants;
using AlphaPlus.Features.Handbook.API.Helpers;
using AlphaPlus.Features.Handbook.API.Models;
using AlphaPlus.Features.Site.Handbook.Requests;
using AlphaPlus.Features.Site.Handbook.ViewModels;
using MediatR;

namespace AlphaPlus.Features.Site.Handbook.Handlers
{
    public class GlossaryHandler :
        APIHandler,
        IRequestHandler<GetGlossaryByGroupRequest, GlossaryViewModel>
    {
        public GlossaryHandler(Fanex.Logging.ILogger logger) : base(logger) { }

        public Task<GlossaryViewModel> Handle(GetGlossaryByGroupRequest request, CancellationToken cancellation)
        {
            var group = request.Group ?? string.Empty;
            var url = contentAPIUrl + "/api/glossaries/latest/g" + group +
                "/" + LanguageConsts.Locales[request.LanguageKey.ToLowerInvariant()] +
                "/" + AppSettingsHelper.SiteLabel +
                "/" + request.RoleId;
            var glossaries = Get<IEnumerable<GlossaryModel>>(url);

            var viewModel = new GlossaryViewModel
            {
                Group = group,
                Glossaries = glossaries
            };

            return Task.FromResult(viewModel);
        }
    }
}