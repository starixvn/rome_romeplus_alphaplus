using AlphaPlus.Products.ViewModels;
using Newtonsoft.Json;

namespace AlphaPlus.Products.Configurations;

public static class ProductManager
{
    private static SettingProductViewModel settingProductViewModel;

    public static SettingProductViewModel ProductViewModel => LoadProducts();

    private static SettingProductViewModel LoadProducts()
    {
        if (settingProductViewModel == null)
        {
            using var sr = new StreamReader($"{AppDomain.CurrentDomain.BaseDirectory}/Products.json");
            string jsonString = sr.ReadToEnd();
            settingProductViewModel = JsonConvert.DeserializeObject<SettingProductViewModel>(jsonString);
        }

        return settingProductViewModel;
    }
}