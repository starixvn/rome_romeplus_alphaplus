namespace AlphaPlus.Settings.Site.Features.Customer.ViewModels;

public class AccountInfoViewModel
{
    public string Usernames { get; set; } = string.Empty;
    
    public decimal Credit { get; set; }
    
    public IEnumerable<string> Domains { get; set; } = Enumerable.Empty<string>();
}