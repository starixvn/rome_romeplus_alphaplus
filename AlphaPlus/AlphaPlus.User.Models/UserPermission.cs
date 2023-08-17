using System;
using AlphaPlus.Configurations;

namespace AlphaPlus.User.Models
{
    public class UserPermission
    {

        public static bool IsShowMixedSport => true;

        public static bool IsShowSportsbook2
        {
            get
            {
                return !CurrentUserSession.GetHideProduct(ProductIdConstants.Sportsbook2);
            }
        }

        public static bool IsShowDoubleCommission
        {
            get
            {
                return CurrentUserSession.IsDoubleCommLosing;
            }
        }


        public static bool IsShowMyanmarOdds
        {
            get
            {
                return CurrentUserSession.IsMyanmarOdds;
            }
        }

        protected static IUserSession CurrentUserSession
        {
            get
            {
                return UserSession.Current;
            }
        }

        public static bool IsEnableProduct(int product)
        {
            return AppSettings.IsEnabledProduct(product) && !CurrentUserSession.GetHideProduct(product);
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
            switch (productId)
            {
                case 1: // sportsbook
                    return true;
                case 16: // SP2
                    return IsShowSportsbook2;
                case 5: // Virtual Sports
                case 44: // RNG Lottery
                    return IsEnableProduct(productId);
                default:
                    return IsEnableProduct(productId) && IsSyncedProduct(productId);
            }
        }
    }
}