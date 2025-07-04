using AlphaPlus.Features.Site.Handbook.Filters;
using AlphaPlus.Features.Site.Handbook.Requests;
using AlphaPlus.Features.Site.Messages.Controllers;
using AlphaPlus.NetThrottling;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace AlphaPlus.Features.Site.Handbook.Controllers
{
    [HandbookFilter, DisableNetThrottling]
    public class HandbookImageController : SiteBaseController
    {
        public HandbookImageController(IMediator mediator) : base(mediator)
        {
        }

        [OutputCache(Duration = 60 * 60 * 24)]
        public async Task<ActionResult> Get(GetImageByNameRequest request)
        {
            var image = await mediator.Send(request);
            if (string.IsNullOrWhiteSpace(image))
            {
                return null;
            }

            var r = "base64,";
            var start = image.IndexOf(r);

            byte[] imageBytes = Convert.FromBase64String(image.Substring(start + r.Length));
            return File(imageBytes, "image/png", request.Name);
        }
    }
}