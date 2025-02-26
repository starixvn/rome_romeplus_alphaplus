using Fanex.Data.Repository;

namespace AlphaPlus.AccountInfo.Site.Features.ProfilePicture.GetLibrary
{
    public class SelectProfilePictureLibraryCriteria(int systemMode) : CriteriaBase
    {
        public override string GetSettingKey() => "Age_CustomerProfilePictureLibrarySel";

        public int Mode { get; } = systemMode;

        public override bool IsValid() => Mode > 0;
    }
} 