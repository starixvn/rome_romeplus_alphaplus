namespace AlphaPlus.Settings.Site.Features.Customer.ViewModels;

public class RedirectPTViewModel(string urlMainPage, string urlMenu)
{
    public string UrlMainPage => urlMainPage;

    public string UrlMenu => urlMenu;
}