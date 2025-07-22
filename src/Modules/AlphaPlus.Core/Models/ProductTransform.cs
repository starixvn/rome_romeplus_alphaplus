using AlphaPlus.Core.Configurations;

namespace AlphaPlus.Core.Models;

public class ProductTransform
{
    private static readonly Dictionary<int, string> ProductNames = LoadProductNameMappings();

    /// <summary>
    /// convention for product:
    /// hide-productid
    /// disable-productid
    /// synced-productid
    /// </summary>
    private static readonly Dictionary<string, bool> StatusDefaultValues = new()
    {
        ["hide-"] = true,
        ["disable-"] = true,
        ["synced-"] = true
    };

    public static void TransformProductStatus(IUserSession session, IDictionary<string, string> values)
    {
        TransformProductStatusByName(session, values);
        TransformProductStatusByConvention(session, values);
    }

    public static void TransformProductStatusByName(IUserSession session, IDictionary<string, string> values)
    {
        if (values == null || !values.Any())
        {
            return;
        }

        foreach (var productName in ProductNames)
        {
            session.SetHideProduct(productName.Key, GetProductStatus("hide" + productName.Value, values, true));
            session.SetDisabledProduct(productName.Key, GetProductStatus("disable" + productName.Value, values, true));
            session.SetSyncedProduct(productName.Key, GetProductStatus("synced" + productName.Value, values, true));
        }
    }

    private static bool GetProductStatus(string productStatus, IDictionary<string, string> values, bool defaultValue)
    {
        if (values.TryGetValue(productStatus ?? string.Empty, out string value))
        {
            return Convert.ToBoolean(value);
        }

        return defaultValue;
    }

    private static void TransformProductStatusByConvention(IUserSession session, IDictionary<string, string> values)
    {
        if (values == null || !values.Any())
        {
            return;
        }

        var productIds = new List<int>();

        foreach (string key in values.Keys)
        {
            var prefix = StatusDefaultValues.FirstOrDefault(statusPrefix => key.StartsWith(statusPrefix.Key));

            if (string.IsNullOrEmpty(prefix.Key)
               || !int.TryParse(key[prefix.Key.Length..], out int productId)
               || productIds.Contains(productId))
            {
                continue;
            }

            productIds.Add(productId);
        }

        foreach (var productId in productIds)
        {
            var prefix = "hide-";
            session.SetHideProduct(productId, GetProductStatus(prefix + productId, values, StatusDefaultValues[prefix]));

            prefix = "disable-";
            session.SetDisabledProduct(productId, GetProductStatus(prefix + productId, values, StatusDefaultValues[prefix]));

            prefix = "synced-";
            session.SetSyncedProduct(productId, GetProductStatus(prefix + productId, values, StatusDefaultValues[prefix]));
        }
    }

    private static Dictionary<int, string> LoadProductNameMappings()
    {
        var result = new Dictionary<int, string>();
        var productText = AppSettings.ProductColumnNameMappings;
        if (!string.IsNullOrEmpty(productText))
        {
            var products = productText.Split(",", StringSplitOptions.RemoveEmptyEntries);
            foreach (var product in products)
            {
                var items = product.Split('=');
                if (items.Length == 2)
                {
                    var id = int.Parse(items[0]);
                    var name = items[1].ToLowerInvariant();

                    result.Add(id, name);
                }
            }
        }

        return result;
    }
}