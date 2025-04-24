using AlphaPlus.Products.ViewModels;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Setting.Base
{
    public interface ISettingItemViewModel : IViewModel
    {
        string Text { get; }

        string Url { get; }
    }
}