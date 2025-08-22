using System.Resources;
using System.Text.RegularExpressions;
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

        public string ReplaceOldNewValue(string trackingValue, Func<string, string> replaceFunc)
        {
            if (string.IsNullOrWhiteSpace(trackingValue))
            {
                return trackingValue;
            }

            // Case 1: Pattern to match: <span class="valuechanged">{newValue}</span><span class="valueold">({oldValue})</span>
            var pattern1 = @"<span class=""valuechanged"">([^<]*)</span><span class=""valueold"">\(([^)]*)\)</span>";
            var match1 = Regex.Match(trackingValue, pattern1);

            if (match1.Success)
            {
                var newValue = match1.Groups[1].Value.Trim();
                var oldValue = match1.Groups[2].Value.Trim();
                return $"<span class=\"valuechanged\">{replaceFunc(newValue)}</span><span class=\"valueold\">({replaceFunc(oldValue)})</span>";
            }

            // Case 2: Pattern to match: <span class="valuechanged" title="{oldValue} --> {newValue}">{newValue}</span>
            var pattern2 = @"<span class=""valuechanged"" title=""([^""]*) --> ([^""]*)"">([^<]*)</span>";
            var match2 = Regex.Match(trackingValue, pattern2);

            if (match2.Success)
            {
                var oldValue = match2.Groups[1].Value.Trim();
                var newValue = match2.Groups[2].Value.Trim();
                return $"<span class=\"valuechanged\" title=\"{replaceFunc(oldValue)} --> {replaceFunc(newValue)}\">{replaceFunc(newValue)}</span>";
            }

            // If the pattern doesn't match, return the original value as new value and empty as old value
            return replaceFunc(trackingValue.Trim());
        }

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

        public string GetProductNames(string productIds)
        {
            var productNames = productIds
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(productId => GetProductName(int.Parse(productId)));

            return string.Join(", ", productNames);
        }

        public string GetTrackingBankName(string code)
        {
            return ReplaceOldNewValue(code, GetBankName);
        }

        private string GetBankName(string code)
        {
            return GetLogResource("BankName_" + code);
        }
    }
}
