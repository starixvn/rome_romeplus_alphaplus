namespace AlphaPlus.Settings.Site.ViewLogs.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using AlphaPlus.Settings.ViewLogs.API.Constants;

    public static class TrackingValueTransformer
    {
        private static readonly Dictionary<string, Func<string, string>> funcDictionary
            = new Dictionary<string, Func<string, string>>
            {
                ["220000,220001,220002,220003,220004,220005,220006"] = TransformParlayMaxPayoutTrackingValue,
                ["220020,220021,220022,220023,220024,220025,220026"] = TransformBetSettingTrackingValue
            };

        public static string TransformToStandardTrackingValue(int actionId, string trackingValue)
        {
            var key = funcDictionary.Keys.SingleOrDefault(
                k => k.Split(new[] { ',' }).Contains(actionId.ToString(), StringComparer.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(key))
            {
                return funcDictionary[key](trackingValue);
            }

            return trackingValue;
        }

        private static string TransformParlayMaxPayoutTrackingValue(string trackingValue)
            => TransformTrackingValue(trackingValue, FormatParlayMaxPayoutAttributeName);

        private static string TransformBetSettingTrackingValue(string trackingValue)
            => TransformTrackingValue(trackingValue, FormatBetSettingAttributeName);

        private static string TransformTrackingValue(string trackingValue, Func<int, int, string, string> formatAttributeName)
        {
            var trackingValueDoc = new XmlDocument
            {
                InnerXml = trackingValue
            };

            var rootElement = trackingValueDoc.DocumentElement;
            foreach (XmlNode childNode in rootElement.ChildNodes)
            {
                var betType = Convert.ToInt32(childNode.Attributes[AttributeName.BetType].Value);
                var sportType = Convert.ToInt32(childNode.Attributes[AttributeName.SportType].Value);
                foreach (XmlAttribute attribute in childNode.Attributes)
                {
                    if (!AttributeName.IgnoredAttributes.Contains(attribute.Name, StringComparer.OrdinalIgnoreCase))
                    {
                        rootElement.SetAttribute(formatAttributeName(betType, sportType, attribute.Name), attribute.Value);
                    }
                }
            }

            rootElement.InnerXml = string.Empty;
            rootElement.IsEmpty = true;
            return rootElement.OuterXml;
        }

        private static string FormatParlayMaxPayoutAttributeName(int betType, int sportType, string attributeName)
        {
            if (sportType == SportId.Soccer)
            {
                return string.Format("Soccer{0}", attributeName);
            }

            if (sportType == SportId.MixSports)
            {
                return string.Format("MixSports{0}", attributeName);
            }

            return attributeName;
        }

        private static string FormatBetSettingAttributeName(int betType, int sportType, string attributeName)
        {
            var shortenedAttributeName = ShortenAttributeName(attributeName);
            if (betType == BetTypeId.Parlay)
            {
                return string.Concat(shortenedAttributeName, "v", sportType, "mp");
            }

            if (sportType == SportId.SabaBasketball)
            {
                return string.Concat(shortenedAttributeName, "v2saba");
            }

            if (sportType == SportId.SabaSoccer)
            {
                return string.Concat(shortenedAttributeName, "v1saba");
            }

            if (sportType != SportId.Soccer)
            {
                return string.Concat(shortenedAttributeName, "v", sportType);
            }

            return shortenedAttributeName;
        }

        private static string ShortenAttributeName(string attributeName)
        {
            string shortenedAttributeName;
            if (AttributeName.ShortenedAttributes.TryGetValue(attributeName, out shortenedAttributeName))
            {
                return shortenedAttributeName;
            }

            return attributeName;
        }
    }
}