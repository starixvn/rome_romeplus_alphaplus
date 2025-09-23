namespace AlphaPlus.Features.Site.Handbook.Requests
{
    public class SportBookInformationRequest : BaseHandbookRequest
    {
        public string TabName { get; set; }
        public int Currency { get; set; }
        public string SiteLabel { get; set; }
    }
}
