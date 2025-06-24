using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Base;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting
{
    public class EditInfoViewModel : SettingItemViewModel
    {
        public EditInfoViewModel(ISearchAccountInfoRequest request, int displayOrder = 1)
            : base(request, displayOrder)

        {
        }

        protected override string BaseAddress => MicroSites.SettingUrl("~/Customer/UpdateInfo");

        public override Dictionary<string, string> QueryStringParameters
            => new()
            {
                ["custId"] = SearchAccount.Id.ToString(),
                ["username"] = SearchAccount.Username,
                ["isCashMember"] = SearchAccount.IsCashMember.ToString(),
            };
    }
}