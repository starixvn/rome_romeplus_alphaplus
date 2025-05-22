using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;
using AlphaPlus.Settings.ViewLogs.API.Constants;
using AlphaPlus.Settings.ViewLogs.API.Params;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Settings.ViewLogs.API.Services.Impl
{
    public class TransformServiceImpl(LogFormatter logFormatter, IMemoryCache cacheService) : TransformService
    {
        private const string TransformObjectCacheKey = "AlphaPlus.Settings.ViewLogs.API.TransformObject_{0}";

        private readonly LogFormatter logFormatter = logFormatter;
        private readonly IMemoryCache cacheService = cacheService;

        public string TransformTrackingValue(
            string trackingValue,
            int logActionId,
            TransformParam transformParam,
            bool isHideNickname = false,
            bool isSupportPotentialWinLimit = false)
        {
            if (transformParam == null ||
                string.IsNullOrWhiteSpace(trackingValue)
                || logActionId < 0)
            {
                return string.Empty;
            }

            var compiledTransform = GetCompiledTransformTemplate(logActionId, transformParam.TrackingValueTransformTemplatePath);

            if (compiledTransform == null)
            {
                return null;
            }

            // Need be reviewed code here, AssignParams should not in formatter class
            logFormatter.Init(transformParam, isHideNickname, isSupportPotentialWinLimit);

            var xsltArguments = new XsltArgumentList();
            xsltArguments.AddExtensionObject("urn:LogFormat", logFormatter);

            // Transform xml must be lower case
            using MemoryStream stream = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(trackingValue.ToLowerInvariant()));
            var document = new XPathDocument(stream);
            using var writer = new StringWriter();
            compiledTransform.Transform(document, xsltArguments, writer);
            return writer.ToString();
        }

        private XslCompiledTransform GetCompiledTransformTemplate(int actionId, string trackingValueTransformTemplatePath)
        {
            var cacheKey = string.Format(TransformObjectCacheKey, actionId);
            var cachedCompiledTransform = cacheService.Get<XslCompiledTransform>(cacheKey);

            if (cachedCompiledTransform != null)
            {
                return cachedCompiledTransform;
            }

            var compiledTransform = new XslCompiledTransform();
            var path = FindTemplatePathInDirectoryByActionId(actionId, trackingValueTransformTemplatePath);

            if (File.Exists(path))
            {
                compiledTransform.Load(path, new XsltSettings(true, true), new XmlUrlResolver());
            }

            if (compiledTransform.OutputSettings == null)
            {
                return null;
            }

            cacheService.Set(cacheKey, compiledTransform, DefaultVariables.CacheItemOptions);

            return compiledTransform;
        }

        private static string FindTemplatePathInDirectoryByActionId(int actionId, string directory)
        {
            if (!Directory.Exists(directory))
            {
                return string.Empty;
            }

            var path = Path.Combine(directory, $"action_{actionId}.xslt");

            if (File.Exists(path))
            {
                return path;
            }

            var subFolders = Directory.GetDirectories(directory);
            foreach (var subFolder in subFolders)
            {
                path = FindTemplatePathInDirectoryByActionId(actionId, Path.Combine(directory, subFolder));

                if (!string.IsNullOrWhiteSpace(path))
                {
                    return path;
                }
            }

            return string.Empty;
        }
    }
}