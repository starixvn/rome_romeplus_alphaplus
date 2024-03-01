namespace AlphaPlus.Settings.Site.ViewModels
{
    using System.Web.Mvc;
    using AlphaPlus.Settings.MemberInfo.Domain.Models.MemberRequests;

    public class ProductViewModel : SelectListItem
    {
        public ProductViewModel(
            string text,
            string value,
            bool selected,
            bool disabled,
            GetSettingResult setting)
        {
            Text = text;
            Value = value;
            Selected = selected;
            Disabled = disabled;

            Setting = setting;
        }

        public GetSettingResult Setting { get; }
    }
}