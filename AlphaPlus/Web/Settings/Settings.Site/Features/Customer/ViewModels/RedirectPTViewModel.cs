namespace AlphaPlus.Settings.Site.Features.Customer.ViewModels;

public class RedirectPTViewModel(string urlMainPage, string urlMenu, IEnumerable<string> productList)
{
    public string UrlMainPage => urlMainPage;

    public string UrlMenu => urlMenu;

    public IEnumerable<string> ProductList => productList;
}