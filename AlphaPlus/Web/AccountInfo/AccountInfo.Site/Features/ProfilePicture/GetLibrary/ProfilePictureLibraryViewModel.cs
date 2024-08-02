using JW;

namespace AlphaPlus.AccountInfo.Site.Features.ProfilePicture.GetLibrary
{
    public class ProfilePictureLibraryViewModel(IEnumerable<ProfilePictureModel> images, Pager pager)
    {
        public IEnumerable<ProfilePictureModel> Images { get; private set; } = images;

        public Pager Pager { get; set; } = pager;
    }
}