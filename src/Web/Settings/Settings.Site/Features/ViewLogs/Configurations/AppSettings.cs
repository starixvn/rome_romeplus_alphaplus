using AlphaPlus.Settings.Site.Features.ViewLogs.Helpers;

namespace AlphaPlus.Settings.Site.Features.ViewLogs.Configurations
{
    using AlphaPlus.Core.Configurations;

    public static class AppSettings
    {
        private static readonly object LockObjectStatusMappings = new object();
        private static readonly object LockObjectResourcesByBetTypeCacheRef = new object();
        private static readonly object LockObjectResourcesBySportTypeCacheRef = new object();

        private static Dictionary<string, int> statusMappings;
        private static Dictionary<string, int> resourcesByBetTypeCacheRef;
        private static Dictionary<string, int> resourcesBySportTypeCacheRef;
        public static string VirtualDirectory
        {
            get
            {
                return AppPathHelper.GetAppPath("/settings/", "~/");
            }
        }

        public static string OldMainAppPath
        {
            get
            {
                return "/settings";
            }
        }

        public static string AccessDeniedPage
        {
            get
            {
                return GetConfigValue("AccessDeniedPage");
            }
        }

        /// <summary>
        /// Key: statusName. Value: productId
        /// </summary>
        public static Dictionary<string, int> StatusMappings
        {
            get
            {
                if (statusMappings == null)
                {
                    InitStatusMappings();
                }

                return statusMappings;
            }
        }

        /// <summary>
        /// Key: statusName. Value: betTypeId
        /// </summary>
        public static Dictionary<string, int> ResourcesByBetTypeCacheRef
        {
            get
            {
                if (resourcesByBetTypeCacheRef == null)
                {
                    InitResourcesByBetTypeCacheRef();
                }

                return resourcesByBetTypeCacheRef;
            }
        }

        /// <summary>
        /// Key: statusName. Value: betTypeId
        /// </summary>
        public static Dictionary<string, int> ResourcesBySportTypeCacheRef
        {
            get
            {
                if (resourcesBySportTypeCacheRef == null)
                {
                    InitResourcesBySportTypeCacheRef();
                }

                return resourcesBySportTypeCacheRef;
            }
        }

        public static string StoredProcedurePath
        {
            get
            {
                return GetConfigValue("StoredProcedurePath");
            }
        }

        public static string FanexDataServiceApi => GetConfigValue("FanexDataServiceApi");

        public static string RemoteRefURL => GetConfigValue("RemoteRefURL");

        public static string GetConfigValue(string configName)
        {
            return ConfigurationManager.GetValue<string>(configName);
        }

        private static void InitStatusMappings()
        {
            lock (LockObjectStatusMappings)
            {
                if (statusMappings == null)
                {
                    statusMappings = new Dictionary<string, int>();
                    var statusMappingString = GetConfigValue("ViewLogs:StatusMapping");

                    if (string.IsNullOrWhiteSpace(statusMappingString))
                    {
                        return;
                    }

                    statusMappingString.Split(',')
                        .ToList()
                        .ForEach((combinedValue) =>
                        {
                            //// format: statusName:productId
                            var keyValue = combinedValue.Split(':');

                            if (keyValue.Length > 1)
                            {
                                statusMappings.Add(keyValue[0].ToLower(), int.Parse(keyValue[1]));
                            }
                        });
                }
            }
        }

        private static void InitResourcesByBetTypeCacheRef()
        {
            lock (LockObjectResourcesByBetTypeCacheRef)
            {
                if (resourcesByBetTypeCacheRef == null)
                {
                    resourcesByBetTypeCacheRef = new Dictionary<string, int>();
                    var resourcesByBetTypeCacheRefSetting = GetConfigValue("ViewLogs:ResourcesByBetTypeCacheRef");

                    if (string.IsNullOrWhiteSpace(resourcesByBetTypeCacheRefSetting))
                    {
                        return;
                    }

                    resourcesByBetTypeCacheRefSetting.Split(',')
                        .ToList()
                        .ForEach((combinedValue) =>
                        {
                            //// format: statusName:productId
                            var keyValue = combinedValue.Split(':');

                            if (keyValue.Length > 1)
                            {
                                resourcesByBetTypeCacheRef.Add(keyValue[0].ToLower(), int.Parse(keyValue[1]));
                            }
                        });
                }
            }
        }

        private static void InitResourcesBySportTypeCacheRef()
        {
            lock (LockObjectResourcesBySportTypeCacheRef)
            {
                if (resourcesBySportTypeCacheRef == null)
                {
                    resourcesBySportTypeCacheRef = new Dictionary<string, int>();
                    var resourcesBySportTypeCacheRefSetting = GetConfigValue("ViewLogs:ResourcesBySportTypeCacheRef");

                    if (string.IsNullOrWhiteSpace(resourcesBySportTypeCacheRefSetting))
                    {
                        return;
                    }

                    resourcesBySportTypeCacheRefSetting.Split(',')
                        .ToList()
                        .ForEach((combinedValue) =>
                        {
                            //// format: statusName:productId
                            var keyValue = combinedValue.Split(':');

                            if (keyValue.Length > 1)
                            {
                                resourcesBySportTypeCacheRef.Add(keyValue[0].ToLower(), int.Parse(keyValue[1]));
                            }
                        });
                }
            }
        }
    }
}