using AlphaPlus.Settings.Site.Features.Customer.Requests;

namespace AlphaPlus.Settings.Site.Features.Customer.ViewModels
{
    public class SportBookInformationViewModel : CustomerBaseViewModel
    {
        public SportBookInformationViewModel(
            SportBookInformationQuery query,
            AppConfiguration appConfig,
            string cndUri,
            string assetsVersion)
            : base(appConfig, cndUri, assetsVersion)
        {
            Params = query.ToJson();
        }
    }
}