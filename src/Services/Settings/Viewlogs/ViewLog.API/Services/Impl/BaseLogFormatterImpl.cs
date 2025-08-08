using System.Resources;
using AlphaPlus.Core.Configurations;
using AlphaPlus.Settings.ViewLogs.API.Params;
using AlphaPlus.Shared.AppResources.Settings.ViewLog;

namespace AlphaPlus.Settings.ViewLogs.API.Services.Impl
{
    public class BaseLogFormatterImpl : BaseLogFormatter
    {
        public ResourceManager ResourceManager { get; set; } = TrackingValueResources.ResourceManager;

        protected readonly RemotingRefWrapper remotingRef;
        protected readonly ViewLogIPService ipService;
        
        public BaseLogFormatterImpl(RemotingRefWrapper remotingRef, ViewLogIPService ipService)
        {
            this.remotingRef = remotingRef;
            this.ipService = ipService;
        }

        public virtual void Init(TransformParam transformParam, bool isHideNickname = false) { }
    
        public string FormatIP(string ip)
        {
            return ipService.FormatIP(ip);
        }

        public string GetLogResource(string key)
        {
            ResourceManager.IgnoreCase = true;
            var value = ResourceManager.GetString(key) ?? string.Empty;
            if (AppSettings.IsSiteLabel777 && key.Contains("Saba"))
            {
                value = ResourceManager.GetString(key.Replace("Saba", "Virtual")) ?? value;
            }

            return value;
        }

        public string GetProductName(int productId)
        {
            return remotingRef.GetProductName(productId);
        }
    }
}
