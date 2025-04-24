using Fanex.Data.Repository;

namespace AlphaPlus.Main.Site.Features.Banner
{
    public class GetBannerCriteria : CriteriaBase
    {
        public GetBannerCriteria(
            string languageCode,
            string environments,
            string systems,
            byte type)
        {
            LanguageCode = languageCode;
            Environments = environments;
            Systems = systems;
            Type = type;
        }

        public string LanguageCode { get; }

        public string Environments { get; }

        public string Systems { get; }

        public byte Type { get; }

        public override string GetSettingKey()
            => "Age_Banner_SelectRandomBanner";

        public override bool IsValid()
            => !string.IsNullOrWhiteSpace(LanguageCode)
            && !string.IsNullOrWhiteSpace(Environments)
            && !string.IsNullOrWhiteSpace(Systems)
            && Type == BannerType.Desktop || Type == BannerType.Mobile;
    }
}
