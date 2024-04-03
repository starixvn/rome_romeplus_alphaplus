namespace AlphaPlus.Settings.Site.ViewLogs.Helpers
{
    using System;
    using System.Linq;
    using System.Web;
    using System.Xml;
    using AlphaPlus.Settings.ViewLogs.API.Constants;
    using AlphaPlus.Settings.ViewLogs.API.Helpers;
    using AlphaPlus.Settings.ViewLogs.API.Models;

    public class TrackingValueFormatterImpl : TrackingValueFormatter
    {
        public string GetFormattedDiffTrackingValue(Log log, bool isShowOldValue)
        {
            if (log == null ||
                string.IsNullOrWhiteSpace(log.TrackingValue))
            {
                return string.Empty;
            }

            var trackingValue = StandardizeTrackingValue(log.ActionId, log.TrackingValue);
            var trackingValueOld = StandardizeTrackingValue(log.ActionId, log.TrackingValueOld);

            var trackingValueDoc = new XmlDocument();
            trackingValueDoc.InnerXml = trackingValue;

            var hasOldXml = !string.IsNullOrWhiteSpace(trackingValueOld);
            var trackingValueOldDoc = hasOldXml ? new XmlDocument() : trackingValueDoc;
            trackingValueOldDoc.InnerXml = hasOldXml ? trackingValueOld : trackingValueDoc.InnerXml;

            FormatDiffAttributes(trackingValueDoc.DocumentElement.Attributes, trackingValueOldDoc.DocumentElement.Attributes, isShowOldValue);

            var childNodeCount = trackingValueDoc.DocumentElement.ChildNodes.Count;

            for (int i = 0; i < childNodeCount; i++)
            {
                var newNodes = trackingValueDoc.DocumentElement.ChildNodes[i];
                var oldNodes = trackingValueOldDoc.DocumentElement.ChildNodes[i];
                FormatChildNode(newNodes, oldNodes, isShowOldValue);
            }

            return trackingValueDoc.InnerXml;
        }

        private void FormatChildNode(XmlNode newNode, XmlNode oldNode, Boolean isShowOldValue)
        {
            FormatDiffAttributes(newNode.Attributes, oldNode.Attributes, isShowOldValue);

            var childNodeCount = newNode.ChildNodes.Count;
            if (childNodeCount == 0)
            {
                return;
            }

            for (int i = 0; i < childNodeCount; i++)
            {
                FormatChildNode(newNode.ChildNodes[i], oldNode.ChildNodes[i], isShowOldValue);
            }
        }

        private static void FormatDiffAttributes(
            XmlAttributeCollection newAttributes,
            XmlAttributeCollection oldAttributes,
            bool isShowOldValue)
        {
            if (newAttributes == null)
            {
                return;
            }

            var ignoreFormatAttributes = new[] { "ip", "loginip", "stakelevel1" };

            foreach (XmlAttribute newAttribute in newAttributes)
            {
                var newValue = newAttribute.Value;

                var oldAttribute = oldAttributes[newAttribute.Name];
                var oldValue = oldAttribute == null ? newValue : oldAttribute.Value;

                newValue = StandardizeAttributeValue(newValue);
                oldValue = StandardizeAttributeValue(oldValue);

                var isAttributeNeedBeFormatted = (!string.IsNullOrWhiteSpace(newValue) || !string.IsNullOrWhiteSpace(oldValue)) &&
                    !ignoreFormatAttributes.Contains(newAttribute.Name.ToLower()) &&
                    !string.Equals(newValue, oldValue, StringComparison.OrdinalIgnoreCase);

                if (!isAttributeNeedBeFormatted)
                {
                    newAttributes[newAttribute.Name].Value = newValue;
                    continue;
                }

                newAttributes[newAttribute.Name].Value = FormatValue(newValue, oldValue, isShowOldValue);
            }
        }

        private static string FormatValue(string newValue, string oldValue, bool isShowOldValue)
        {
            string formattedValue = isShowOldValue
                ? string.Format("<span class=\"valuechanged\">{0}</span><span class=\"valueold\">({1})</span>", newValue, oldValue)
                : string.Format("<span class=\"valuechanged\" title=\"{0} --> {1}\">{1}</span>", oldValue, newValue);

            return formattedValue;
        }

        private static string StandardizeTrackingValue(int actionId, string trackingValue)
        {
            var result = TrackingValueTransformer.TransformToStandardTrackingValue(actionId, trackingValue);

            if (!string.IsNullOrWhiteSpace(result))
            {
                result = result.Replace("\a", string.Empty);
            }

            return result;
        }

        private static bool IsPermissionValue(string attrValue)
        {
            return attrValue.Contains("[") && attrValue.Contains("]");
        }

        private static string StandardizeAttributeValue(string attrValue)
        {
            if (string.IsNullOrWhiteSpace(attrValue))
            {
                return attrValue;
            }

            var standardizedAttributeValue = attrValue;
            var isPermissionAttribute = IsPermissionValue(attrValue);

            if (isPermissionAttribute)
            {
                standardizedAttributeValue = FormatPermissionValue(standardizedAttributeValue);
            }

            standardizedAttributeValue = StringHelper.RemoveTrailingZeros(standardizedAttributeValue);
            standardizedAttributeValue = HttpUtility.HtmlEncode(standardizedAttributeValue);

            return standardizedAttributeValue;
        }

        private static string FormatPermissionValue(string attrValue)
        {
            var standardizedAttributeValue = attrValue.ToUpper();

            var pemissionAbbriations = Permissions.PemissionAbbriations;

            foreach (var pemissionAbbriation in pemissionAbbriations)
            {
                if (standardizedAttributeValue.Contains(pemissionAbbriation.Key))
                {
                    standardizedAttributeValue = standardizedAttributeValue.Replace(pemissionAbbriation.Key, pemissionAbbriation.Value);
                }
            }

            var pageAccessTypeNames = Permissions.PageAccessTypeName;

            foreach (var pageAccessTypeName in pageAccessTypeNames)
            {
                if (standardizedAttributeValue.Contains(pageAccessTypeName.Key))
                {
                    standardizedAttributeValue = standardizedAttributeValue.Replace(pageAccessTypeName.Key, "[" + pageAccessTypeName.Value + "]");
                }
            }

            return standardizedAttributeValue;
        }
    }
}