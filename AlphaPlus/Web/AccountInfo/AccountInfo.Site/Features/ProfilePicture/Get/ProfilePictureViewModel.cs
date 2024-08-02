using AlphaPlus.AccountInfo.Site.Features.ProfilePicture.GetLibrary;
using AlphaPlus.Core.Models;

namespace AlphaPlus.AccountInfo.Site.Features.ProfilePicture.Get
{
    public class ProfilePictureViewModel
    {
        public ProfilePictureViewModel(
            IEnumerable<ProfilePictureModel> images,
            byte actorRoleId,
            bool allowUpdate,
            bool enableSecurityCode,
            bool isOfferProtectedMode,
            string displayLanguage,
            IDictionary<string, string> languages,
            bool allowUpdateDisplayLanguage)
        {
            Images = images;
            AllowUpdate = allowUpdate;
            ActorLevel = Enumeration.FromValue<UserLevel>(actorRoleId).DisplayName;
            EnableSecurityCode = enableSecurityCode;
            IsOfferProtectedMode = isOfferProtectedMode;
            DisplayLanguage = displayLanguage;
            Languages = languages;
            AllowUpdateDisplayLanguage = allowUpdateDisplayLanguage;
        }

        public IEnumerable<ProfilePictureModel> Images { get; private set; }

        public string ActorLevel { get; private set; }

        public bool AllowUpdate { get; private set; }

        public bool EnableSecurityCode { get; }

        public bool IsOfferProtectedMode { get; }

        public string DisplayLanguage { get; }

        public IDictionary<string, string> Languages { get; }

        public bool AllowUpdateDisplayLanguage { get; }
    }
}