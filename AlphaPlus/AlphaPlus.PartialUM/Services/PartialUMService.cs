namespace AlphaPlus.PartialUM.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.PartialUM.Configurations;
    using AlphaPlus.PartialUM.DataAccess;
    using AlphaPlus.PartialUM.Exceptions;
    using AlphaPlus.PartialUM.Helpers;
    using AlphaPlus.PartialUM.Models;
    using AlphaPlus.User.Models;
    using Fanex.Caching;

    public interface IPartialUMService
    {
        void ThrowIfUM(string settingKey);

        void ThrowIfUM(string settingKey, string siteName);
    }

    public class PartialUMService : IPartialUMService
    {
        private readonly IPartialUMDataAccess partialUMDataAccess;
        private readonly IPartialUMSectionHelper partialUMSectionHelper;

        public PartialUMService(IPartialUMDataAccess partialUMDataAccess, IPartialUMSectionHelper partialUMSectionHelper)
        {
            this.partialUMDataAccess = partialUMDataAccess;
            this.partialUMSectionHelper = partialUMSectionHelper;
        }

        public void ThrowIfUM(string settingKey)
            => ThrowIfUM(settingKey, UserSession.Current.Site);

        public void ThrowIfUM(string settingKey, string siteName)
        {
            var functionNames = partialUMSectionHelper.GetFunctionNames(settingKey);
            if (!functionNames.Any())
            {
                return;
            }

            var umInfoDictionary = GetUMInfoDictionary(siteName);
            if (umInfoDictionary.Count == 0)
            {
                return;
            }

            foreach (var function in functionNames)
            {
                if (umInfoDictionary.TryGetValue(function, out var umInfo))
                {
                    throw new PartialUMException(FormatUMTime(umInfo.BeginTime), FormatUMTime(umInfo.EndTime));
                }
            }
        }

        private IDictionary<string, UMInfo> GetUMInfoDictionary(string siteName)
            => ConfiguredCacheService
                .GetOrSet(
                $"AlphaPlus.PartialUM.Services.MinimizedUM.SiteName-{siteName}",
                () => GetUMInfoDictionaryFromDB(siteName),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(AppSettings.PartialUMReportsBetListsCacheInSeconds)));

        private IDictionary<string, UMInfo> GetUMInfoDictionaryFromDB(string siteName)
        {
            var umInfoList = partialUMDataAccess.GetUMInfoList(siteName);
            var umInfoDictionary = new Dictionary<string, UMInfo>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in umInfoList)
            {
                umInfoDictionary[item.FunctionTypeName] = item;
            }

            return umInfoDictionary;
        }

        private static string FormatUMTime(DateTime dateTime)
            => dateTime.ToString("dd/MM/yyyy hh:mm tt");
    }
}