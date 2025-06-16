using System.Collections;
using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.Settings.ViewLog;

namespace AlphaPlus.Settings.ViewLogs.API.Constants
{
    public static class Permissions
    {
        private const int ExcludeBracketIndex = 2;

        private static readonly IDictionary<string, string> PemissionAbbs = new Dictionary<string, string>
        {
            [PageAccessType.CreateAccount + PageAccessType.UpdateAccount + PageAccessType.ViewMemberInfo] = PageAccessType.CreateAccount,
            [PageAccessType.CreateAccount + PageAccessType.UpdateAccount] = PageAccessType.CreateAccount,
            [PageAccessType.UpdateAccount + PageAccessType.ViewMemberInfo] = PageAccessType.UpdateAccount
        };

        public static IDictionary<string, string> PemissionAbbriations
        {
            get
            {
                return PemissionAbbs;
            }
        }

        public static IDictionary<string, string> PageAccessTypeName
        {
            get
            {
                // Multiple language => must create every time when user gets text
                var permissions = new Dictionary<string, string>();

                foreach (DictionaryEntry accessType in PageAccessType.AccessTypes)
                {
                    var key = accessType.Key?.ToString() ?? string.Empty;
                    permissions.Add(key, Resources.ResourceManager.GetString("PageAccess_" + GetShortPermissionKey(key)) ?? string.Empty);
                }

                foreach (var accessType in PageAccessType.CashServiceAccessTypes)
                {
                    permissions.Add(accessType, Resources.ResourceManager.GetString("PageAccess_" + GetShortPermissionKey(accessType)) ?? string.Empty);
                }

                return permissions;
            }
        }

        private static string GetShortPermissionKey(string key)
        {
            return key.Substring(1, key.Length - ExcludeBracketIndex);
        }
    }
}
