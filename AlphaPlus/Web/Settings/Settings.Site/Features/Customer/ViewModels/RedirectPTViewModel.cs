using AlphaPlus.Products.Models;

namespace AlphaPlus.Settings.Site.Features.Customer.ViewModels;

public class RedirectPTViewModel(string urlMainPage, IEnumerable<SettingProductTab> productList)
{
    public string UrlMainPage => urlMainPage;

    public IEnumerable<SettingProductTab> ProductList => productList;
}