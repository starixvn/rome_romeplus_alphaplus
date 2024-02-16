namespace AlphaPlus.Core.Models;

public class UserPermission
{

    protected static IUserSession CurrentUserSession
    {
        get
        {
            return UserSession.Current;
        }
    }

    public static bool IsEnableProduct(int product)
    {
        return !CurrentUserSession.GetHideProduct(product);
    }

    public static bool IsSyncedProduct(int product)
    {
        return CurrentUserSession.GetSyncedProduct(product);
    }

    public static bool IsDisabledProduct(int product)
    {
        return CurrentUserSession.GetDisabledProduct(product);
    }

    public static bool HasPermissionOnProduct(int productId)
    {
        return productId switch
        {
            // sportsbook
            1 => true,
            // Virtual Sports
            5 or 44 => IsEnableProduct(productId),
            _ => IsEnableProduct(productId) && IsSyncedProduct(productId),
        };
    }
}