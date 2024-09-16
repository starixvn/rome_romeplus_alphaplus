using AlphaPlus.Products.Models;

namespace AlphaPlus.Settings.Site.Features.Customer.ViewModels;

public class RedirectPTViewModel(string urlMainPage, string urlMenu, IEnumerable<SettingProductTab> productList)
{
    public string UrlMainPage => urlMainPage;

    public string UrlMenu => urlMenu;

    public IEnumerable<SettingProductTab> ProductList => productList;
}