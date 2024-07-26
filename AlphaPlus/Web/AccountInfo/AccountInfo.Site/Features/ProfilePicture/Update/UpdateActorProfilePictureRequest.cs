using AlphaPlus.AccountInfo.Site.Features.Shared;
using AlphaPlus.Core.Models;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.ProfilePicture.Update
{
    public class UpdateActorProfilePictureRequest : BaseQuery, IRequest<Result>
    {
    }
}