namespace Fanex.Razorbill.Security.Customer
{
    using System;
    using System.Text.RegularExpressions;
    using System.Web;
    using AlphaPlus.User.Models;
    using Fanex.CustomerSetting.Client40;
    using Fanex.CustomerSetting.Client40.Models;
    using Fanex.Razorbill.Security.Helpers;

    public class CustomerSettingClientRegistration : CustomerSettingRegistration
    {
        public override bool VerifyQueryPermission(PermissionContext permissionContext)
        {
            return HasPermissionOnProductId(permissionContext);
        }

        public override bool VerifyUpdatePermission(PermissionContext permissionContext)
        {
            return IsAllowPermissionBasedOnSubAccAndInternalAcc() && HasPermissionOnProductId(permissionContext);
        }

        public override bool VerifyMultipleUpdatePermission(PermissionContext permissionContext)
        {
            return IsAllowPermissionBasedOnSubAccAndInternalAcc() && HasPermissionOnProductId(permissionContext);
        }

        public override SettingContext FetchSettingContext()
        {
            var currentUserSession = UserSession.Current;
            var settingContext = new SettingContext()
            {
                ActorId = (int)currentUserSession.UserId,
                ActorName = (string)currentUserSession.UserName,
                ActorLevel = (byte)currentUserSession.RoleId,
                SubAccountId = (int)currentUserSession.SubAccId,
                Language = (string)currentUserSession.Language,
                Domain = HttpContext.Current.Request.Url.Host,
                IP = IPHelper.CustomerIp()
            };

            return settingContext;
        }

        private bool IsAllowPermissionBasedOnSubAccAndInternalAcc()
        {
            var currentUserSession = UserSession.Current;
            bool subAccountPermission = !currentUserSession.IsSubAcc || currentUserSession.SubPageAccess.UpdateAccount;
            return !currentUserSession.IsInternal && subAccountPermission;
        }

        private bool HasPermissionOnProductId(PermissionContext permissionContext)
        {
            var productId = GetProductId(permissionContext);

            return productId == 0 ? true : UserPermission.HasPermissionOnProduct(productId);
        }

        private int GetProductId(PermissionContext permissionContext)
        {
            var queryString = permissionContext.AuthorizationContext.Controller.ValueProvider.GetValue("productId");
            var productId = queryString != null ? Convert.ToInt32(queryString.AttemptedValue) : 0;
            if (productId == 0)
            {
                productId = GetProductIdFromJson(permissionContext);
            }

            return productId;
        }

        private int GetProductIdFromJson(PermissionContext permissionContext)
        {
            var productId = 0;
            var pattern = "(\"productId\":)([^\r\n][0-9]*)";
            var setting = permissionContext.AuthorizationContext.Controller.ValueProvider.GetValue("setting");
            var settingJson = setting != null ? setting.AttemptedValue : string.Empty;

            var regexMatch = Regex.Match(settingJson, pattern, RegexOptions.IgnoreCase);
            if (regexMatch.Success)
            {
                productId = Convert.ToInt32(Regex.Replace(regexMatch.Groups[2].ToString(), @"[^a-zA-Z0-9_.]+", string.Empty, RegexOptions.Compiled));
            }

            return productId;
        }
    }
}