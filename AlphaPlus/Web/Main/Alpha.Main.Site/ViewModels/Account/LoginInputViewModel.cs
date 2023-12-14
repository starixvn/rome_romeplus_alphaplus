using System.ComponentModel.DataAnnotations;

namespace Alpha.Main.Site.ViewModels.Account;

public class LoginInputViewModel
{
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }
    public string ReturnUrl { get; set; }
    public string Language { get; set; }
    public string BrowserSize { get; set; }
}
