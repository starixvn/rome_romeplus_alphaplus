using AlphaPlus.Products.ViewModels;

namespace AlphaPlus.Settings.Site.Features.CustomerList.ViewModels.Status.Base
{
    public interface IStatusItemViewModel : IViewModel
    {
        string Text { get; }

        string Url { get; }

        bool Disabled { get; }

        bool Checked { get; }

        bool ShowInCustomerList { get; }
    }
}