using AlphaPlus.Settings.Site.Features.Shared;

namespace AlphaPlus.Settings.Site.Features.AccountInfo.Requests
{
    public class IndexQuery : BaseQuery
    {
        public bool MobileView { get; set; }

        public bool IsEnabledCustomerDateLimit => viewer.IsEnabledCustomerDateLimit;
    }
}