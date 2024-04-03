namespace AlphaPlus.Settings.ViewLogs.API.Params
{
    using System.Collections.Generic;

    public class TransformParam
    {
        public bool IsShowOldValue { get; set; }

        public string TrackingValueTransformTemplatePath { get; set; }

        public Dictionary<string, bool> CustStatuses { get; set; }

        public Dictionary<string, int> ResourcesProductCacheRef { get; set; }

        public Dictionary<string, int> ResourcesByBetTypeCacheRef { get; set; }

        public Dictionary<string, int> ResourcesBySportTypeCacheRef { get; set; }

        public bool IsShowMyanmarOdds { get; set; }
    }
}
