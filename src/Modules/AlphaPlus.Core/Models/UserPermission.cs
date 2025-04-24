using AlphaPlus.Core.Configurations;

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
        return !AppSettings.ForcedHideProductIds.Contains(product) && !CurrentUserSession.GetHideProduct(product);
    }

    public static bool IsSyncedProduct(int product)
    {
        return CurrentUserSession.GetSyncedProduct(product);
    }

    public static bool IsDisabledProduct(int product)
    {
        return CurrentUserSession.GetDisabledProduct(product);
    }

    // sportsbook is always SHOW
    public static bool HasPermissionOnProduct(int productId)
        => productId == 1 || IsEnableProduct(productId);

    public static bool IsShowOrNotExistProduct(int product)
        => IsEnableProduct(product) || !CurrentUserSession.HasProperty("HIDEPRODUCT_" + product);
}