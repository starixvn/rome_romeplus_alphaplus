using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Base;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting
{
    public class EditRemarkInfoViewModel : SettingItemViewModel
    {
        public EditRemarkInfoViewModel(ISearchAccountInfoRequest request, int displayOrder = 2)
            : base(request, displayOrder)
        {
        }

        protected override string BaseAddress => MicroSites.AccountInfoUrl("~/SearchAccount/RemarkInfo");

        public override Dictionary<string, string> QueryStringParameters
            => new()
            {
                ["searchAccountId"] = SearchAccount.Id.ToString(),
                ["searchAccountName"] = SearchAccount.Username,
            };

        public override bool Visible => true;
    }
}
