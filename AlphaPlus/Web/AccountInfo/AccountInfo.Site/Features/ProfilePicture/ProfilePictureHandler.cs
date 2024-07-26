using AlphaPlus.AccountInfo.Site.Features.ProfilePicture.Get;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.ProfilePicture
{
    public class ProfilePictureHandler : IRequestHandler<ProfilePictureQuery, ProfilePictureViewModel>
    {
        public Task<ProfilePictureViewModel> Handle(ProfilePictureQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}