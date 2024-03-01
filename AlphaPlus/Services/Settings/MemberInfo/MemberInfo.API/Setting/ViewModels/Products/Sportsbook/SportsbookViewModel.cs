namespace AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels.Products
{
    using AlphaPlus.Settings.MemberInfo.API.Requests;
    using Alpha.Products.Core.Configuration;
    using Base;
    using System.Collections.Generic;

    public class SportsbookViewModel : ProductSettingItemViewModel
    {
        public SportsbookViewModel(ISearchAccountInfoRequest request, IConfigurationItem productItem)
            : base(request, productItem)
        {
        }

        public override string Text => request.GetProductName(ProductId);

        public override string Icon
            => request.GetData<bool>(DataKey.Actor.HasUpdateAccountPermission)
            ? "icon-edit Enable"
            : "icon-eye View";

        public override bool Enabled => true;

        public override Dictionary<string, string> QueryStringParamaters
         => new Dictionary<string, string>
         {
             ["m"] = "0",
             ["custId"] = SearchAccount.Id.ToString(),
             ["user"] = SearchAccount.Username,
             [$"{SearchAccount.Upline.CustomerLevel.DisplayName}id"] = SearchAccount.Upline.Id.ToString()
         };

        protected override string BaseAddress
        {
            get
            {
                var useCase = $"{Actor.CustomerLevel.DisplayName}/Edit{SearchAccount.CustomerLevel.DisplayName}";

                return $"{OldMainUrl}_MemberInfo/PositionTaking/{useCase}.aspx";
            }
        }
    }
}