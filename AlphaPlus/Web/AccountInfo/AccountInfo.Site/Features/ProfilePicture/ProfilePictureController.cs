using AlphaPlus.AccountInfo.Site.Features.ProfilePicture.Get;
using AlphaPlus.AccountInfo.Site.Features.ProfilePicture.GetLibrary;
using AlphaPlus.AccountInfo.Site.Features.ProfilePicture.Update;
using AlphaPlus.AccountInfo.Site.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AlphaPlus.AccountInfo.Site.Features.ProfilePicture
{
    public class ProfilePictureController(IMediator mediator)
        : SiteBaseController(mediator)
    {
        public Task<ActionResult> Index(ProfilePictureQuery message)
            => View(message);

        public Task<ActionResult> GetPictureLibrary(ProfilePictureLibraryQuery message)
            => Json(message);

        [HttpPost, ValidateAntiForgeryToken]
        public Task<ActionResult> UpdateActorProfilePicture([FromBody] UpdateActorProfilePictureRequest query)
            => Json(query);
    }
}