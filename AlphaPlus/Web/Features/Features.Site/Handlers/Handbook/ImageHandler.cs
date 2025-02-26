using AlphaPlus.Features.Site.Handbook.Requests;
using MediatR;

namespace AlphaPlus.Features.Site.Handbook.Handlers
{
    public class ImageHandler:
        APIHandler,
        IRequestHandler<GetImageByNameRequest, string>
    {
        public ImageHandler(Fanex.Logging.ILogger logger) : base(logger) { }

        public Task<string> Handle(GetImageByNameRequest request, CancellationToken cancellation)
        {
            var url = contentAPIUrl + "/api/images/base64/" + request.Name;
            var result = Get(url);
            return Task.FromResult(result);
        }
    }
}