using AlphaPlus.Core.Helpers;
using AlphaPlus.Settings.Site.Features.CustomerList.Requests;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Base
{
    public class ProductGroupSettingItemViewModel : SettingItemViewModel
    {
        protected readonly ISearchAccountInfoRequest request;
        private const int ProductGroupStartIndex = 10;

        public ProductGroupSettingItemViewModel(ISearchAccountInfoRequest request, int productGroupId)
           : base(request, ProductGroupStartIndex + productGroupId)
        {
            this.request = request;
            ProductGroupId = productGroupId;
        }

        public int ProductGroupId { get; }

        public override string Text => request.GetProductGroupName(ProductGroupId);

        public override string Icon
            => request.GetData<bool>(DataKey.Actor.HasUpdateAccountPermission)
            ? "icon-edit Enable"
            : "icon-eye View";

        public override bool Enabled => true;

        public override bool Visible => Actor.IsNotSuper || SearchAccount.IsNotAgent;

        public override Dictionary<string, string> QueryStringParameters
         => new Dictionary<string, string>
         {
             ["productGroupId"] = ProductGroupId.ToString(),
             ["custId"] = SearchAccount.Id.ToString(),
             ["roleId"] = SearchAccount.CustomerLevelValue.ToString(),
             ["lng"] = request.Language,
             ["isDarkMode"] = request.IsDarkMode.ToString().ToLowerInvariant()
         };

        protected override string BaseAddress => MicroSites.SettingUrl("~/Customer/EditCustomer");
    }
}