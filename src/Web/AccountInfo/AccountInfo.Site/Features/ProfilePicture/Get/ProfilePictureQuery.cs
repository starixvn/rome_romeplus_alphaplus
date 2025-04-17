using AlphaPlus.Core;
using MediatR;

namespace AlphaPlus.AccountInfo.Site.Features.ProfilePicture.Get
{
    public class ProfilePictureQuery : QueryBase, IRequest<ProfilePictureViewModel>
    {
        private const string DisplayLanguageSessionKey = "languageusage";

        public bool AllowUpdate => CanUpdateAccount && !userSession.IsSubAcc && !userSession.IsInternal;

        public bool IsOfferProtectedMode => userSession.IsOfferProtectedMode;

        public string DisplayLanguage => userSession.GetProperty<string>(DisplayLanguageSessionKey);

        public bool AllowUpdateDisplayLanguage => !userSession.IsInternal;
    }
}