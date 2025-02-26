using AlphaPlus.Core.Models;

namespace AlphaPlus.Products.Models;

public class SettingProduct
{
    private const string HideProduct = "hide-";
    
    private const string SyncedProduct = "synced-";

    public int Id { get; set; }

    public string Name { get; set; }

    public bool Active { get; set; }

    public string CustomerSettingLink { get; set; }

    public bool ShowSettings => 
        UserSession.Current.GetProperty<bool>($"{SyncedProduct}{Id}") && 
        !UserSession.Current.GetProperty<bool>($"{HideProduct}{Id}") &&
        !string.IsNullOrWhiteSpace(CustomerSettingLink);
}