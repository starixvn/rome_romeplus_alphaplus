namespace AlphaPlus.Settings.Site.ViewModels
{
    using AlphaPlus.Settings.MemberInfo.Domain.Models.MemberRequests;
    using Microsoft.AspNetCore.Mvc.Rendering;

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