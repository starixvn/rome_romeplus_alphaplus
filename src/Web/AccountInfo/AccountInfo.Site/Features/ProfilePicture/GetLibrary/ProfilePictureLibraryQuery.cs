using AlphaPlus.Core;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.ProfilePicture.GetLibrary
{
    public class ProfilePictureLibraryQuery : QueryBase, IRequest<ProfilePictureLibraryViewModel>
    {
        public int CurrentPage { get; set; }
    }
}