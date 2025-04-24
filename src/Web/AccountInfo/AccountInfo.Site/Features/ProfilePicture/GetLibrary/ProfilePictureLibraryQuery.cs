using AlphaPlus.AccountInfo.Site.Features.Shared;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.ProfilePicture.GetLibrary
{
    public class ProfilePictureLibraryQuery : BaseQuery, IRequest<ProfilePictureLibraryViewModel>
    {
        public int CurrentPage { get; set; }
    }
}