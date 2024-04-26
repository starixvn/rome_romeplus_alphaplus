namespace AlphaPlus.Settings.ViewLogs.API.Services.Impl
{
    using System.IO;
    using System.Text;
    using System.Xml.XPath;
    using System.Xml.Xsl;
    using Constants;
    using Fanex.Caching;
    using Params;

    public class TransformServiceImpl : TransformService
    {
        private const string TransformObjectCacheKey = "AlphaPlus.Settings.ViewLogs.API.TransformObject_{0}";

        private readonly LogFormatter logFormatter;
        private readonly ICacheService cacheService;

        public TransformServiceImpl(LogFormatter logFormatter, ICacheService cacheService)
        {
            this.logFormatter = logFormatter;
            this.cacheService = cacheService;
        }

        public string? TransformTrackingValue(
            string trackingValue,
            int logActionId,
            TransformParam transformParam,
            bool isHideNickname = false)
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
            logFormatter.Init(transformParam, isHideNickname);

            var xsltArguments = new XsltArgumentList();
            xsltArguments.AddExtensionObject("urn:LogFormat", logFormatter);

            // Transform xml must be lower case
            using (MemoryStream stream = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(trackingValue.ToLowerInvariant())))
            {
                var document = new XPathDocument(stream);
                using (var writer = new StringWriter())
                {
                    compiledTransform.Transform(document, xsltArguments, writer);
                    return writer.ToString();
                }
            }
        }

        private XslCompiledTransform? GetCompiledTransformTemplate(int actionId, string? trackingValueTransformTemplatePath)
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
                compiledTransform.Load(path);
            }

            if (compiledTransform.OutputSettings == null)
            {
                return null;
            }

            cacheService.Set(cacheKey, compiledTransform, DefaultVariables.CacheItemOptions);

            return compiledTransform;
        }

        private string FindTemplatePathInDirectoryByActionId(int actionId, string? directory)
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