using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.Site.Features.CustomerList;
using AlphaPlus.Settings.Site.Features.CustomerList.Requests;
using AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Base;

namespace AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels
{
    public class TransferViewModel : SettingItemViewModel
    {
        public TransferViewModel(ISearchAccountInfoRequest request, int displayOrder = 2)
            : base(request, displayOrder)
        {
        }

        protected override string BaseAddress => MicroSites.SettingUrl("~/TransferCondition");

        public override Dictionary<string, string> QueryStringParameters
          => new()
          {
              ["custId"] = SearchAccount.Id.ToString(),
              ["username"] = SearchAccount.Username
          };

        public override bool Visible
            => base.Visible
            && (SearchAccount.IsMaster
                || (SearchAccount.IsAgent
                     && Request.GetData<bool>(DataKey.Actor.CanEditDailyTransferOption)));
    }
}