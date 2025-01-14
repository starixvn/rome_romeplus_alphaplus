namespace AlphaPlus.Main.Site.Features.ContactUs
{
    public class ContactUsViewModel
    {
        private const string DefaultSkypeId = "maxcsd_agent";
        private const string DefaultLetstalkId = "letstalkid_notexisted";
        private const string DefaultLetstalkUrl = "https://www.letstalk.net/detect.html";

        private readonly string _skypeId;
        private readonly string _letstalkId;
        private readonly string _letstalkUrl;

        public ContactUsViewModel(string domain)
        {
            IsShowSkypeId = AppSettings.SiteLabel.Equals("MBC", StringComparison.OrdinalIgnoreCase);
            _skypeId = AppSettings.ContactUsSkypeId;

            var newDomains = AppSettings.ContactUsNewDomains ?? [];
            var isNewDomain = newDomains
                .Contains(domain, StringComparer.OrdinalIgnoreCase);

            if (isNewDomain)
            {
                _letstalkId = AppSettings.ContactUsLetstalkIdNew;
                _letstalkUrl = AppSettings.ContactUsLetstalkUrlNew;
            }
            else
            {
                _letstalkId = AppSettings.ContactUsLetstalkId;
                _letstalkUrl = AppSettings.ContactUsLetstalkUrl;
            }
        }

        public bool IsShowSkypeId { get; }

        public string SkypeId
            => string.IsNullOrWhiteSpace(_skypeId) ? DefaultSkypeId : _skypeId;

        public string LetstalkId
            => string.IsNullOrWhiteSpace(_letstalkId) ? DefaultLetstalkId : _letstalkId;

        public string LetstalkUrl
            => string.IsNullOrWhiteSpace(_letstalkUrl) ? DefaultLetstalkUrl : _letstalkUrl;
        
        public string ClaimSpotUrl
            => $"https://secure.livechatenterprise.com/customer/action/open_chat?license_id={AppSettings.LiveChatLicenseID}&groups=12&name=";
    }
}