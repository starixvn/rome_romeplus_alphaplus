namespace AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels.Products
{
    using AlphaPlus.Settings.MemberInfo.API.Requests;
    using AlphaPlus.Settings.MemberInfo.API.Setting.ViewModels.Base;
    using Alpha.Products.Core.Configuration;
    using System.Collections.Generic;

    public class SportsbookMultipleViewModel : SportsbookViewModel, ISettingItemMultipleViewModel
    {
        public SportsbookMultipleViewModel(ISearchAccountInfoRequest request, IConfigurationItem productItem)
            : base(request, productItem)
        {
        }

        public override string Text => request.GetResource("EditMultipleSportsbookText");

        public override Dictionary<string, string> QueryStringParamaters
        => new Dictionary<string, string>
        {
            ["m"] = "1",
            ["custId"] = SearchAccount.Id.ToString(),
            ["user"] = SearchAccount.Username,
            [$"{SearchAccount.Upline.CustomerLevel.DisplayName}id"] = SearchAccount.Upline.Id.ToString()
        };

        protected override string BaseAddress
        {
            get
            {
                var useCase = $"{Actor.CustomerLevel.DisplayName}/EditMultiple{SearchAccount.CustomerLevel.DisplayName}s";

                return $"{OldMainUrl}_MemberInfo/PositionTaking/{useCase}.aspx";
            }
        }

        public override string Url => BaseAddress;

        public override string Icon => "icon-edit Enable";

        public override bool ShowGuideLine => true;

        public string GuideLineUrl => $"{OldMainUrl}MemberInformation/MultipleEditSportsBook/GuideLine";
    }
}