using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Base;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Multiple
{
    public class DoubleCommMultipleViewModel : SettingItemViewModel, ISettingItemMultipleViewModel
    {
        private new const int DisplayOrder = 1000;

        public DoubleCommMultipleViewModel(ISearchAccountInfoRequest request) : base(request, DisplayOrder)
        {
        }

        public override string Text => Request.GetResource("EditMultipleDoubleCommText");

        protected override string BaseAddress => MicroSites.SettingUrl("~/CustomerList/MultipleDoubleComm");

        public override Dictionary<string, string> QueryStringParamaters
              => new()
              {
                  ["SearchAccountIds"] = Request.GetData<string>("customerIds"),
                  ["SearchAccountRoleId"] = SearchAccount.CustomerLevelValue.ToString()
              };
    }
}