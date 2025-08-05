using Alpha.Handbook.Demo.Models;
using Alpha.Handbook.Demo.Requests;
using Alpha.Handbook.Demo.ViewModels;
using MediatR;

namespace Alpha.Handbook.Demo.Handlers
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
                "/" + request.Language +
                "/" + request.SiteLabel +
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