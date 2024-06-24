using System.Resources;
using System.Text;
using AlphaPlus.Shared.AppResources.Main;
using AlphaPlus.Main.Site.Extensions;
using Fanex.Caching;
using Fanex.Data.Repository;
using Fanex.RemotingRef;

namespace AlphaPlus.Main.Site.Features.UMMessage
{
    public interface IUMMessageService
    {
        Task<IEnumerable<UMMessageDTO>> GetMessagesAsync(UMMessageParam param);
    }

    public class UMMessageService : IUMMessageService
    {
        private const string UMMessageCacheKey = "Alpha.Main.Services.Impl.UMMessageService";
        private readonly IDynamicRepository dynamicRepository;
        private readonly ResourceManager settingTypeResource;
        private readonly ICacheService cacheService;
        private readonly IRemotingRefService remotingRefService;

        public UMMessageService(IDynamicRepository dynamicRepository, ICacheService cacheService, IRemotingRefService remotingRefService)
        {
            this.dynamicRepository = dynamicRepository;
            this.remotingRefService = remotingRefService;
            this.cacheService = cacheService;
            settingTypeResource = new ResourceManager(typeof(SettingType));
        }

        public async Task<IEnumerable<UMMessageDTO>> GetMessagesAsync(UMMessageParam param)
            => BuildMessages(await GetUMMessageItemAsync(param));

        private async Task<IEnumerable<UMMessageItem>> GetUMMessageItemAsync(UMMessageParam param)
        {
            var cacheKey = $"{UMMessageCacheKey}_{param.Site}_{param.CurrencyId}_{param.SuperId}";
            var messages = cacheService.Get<IEnumerable<UMMessageItem>>(cacheKey);

            if (messages == null)
            {
                messages = await dynamicRepository.FetchAsync<UMMessageItem>(new GetUMMessageCriteria(param.CustId));

                cacheService.Set(
                   cacheKey,
                   messages,
                   new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(AppSettings.UMAnnouncementPopupServerCacheInMinutes)));
            }

            return messages;
        }

        private IEnumerable<UMMessageDTO> BuildMessages(IEnumerable<UMMessageItem> messages)
        {
            foreach (var message in messages)
            {
                switch (message.FunctionType)
                {
                    case UMFunctionType.LockEdit:
                        yield return BuildCustomerSettingsMessage(message);
                        break;

                    case UMFunctionType.NewAccount:
                        yield return BuildUMPatternFromTo(Resources.UMNewAccountPattern, message);
                        break;

                    case UMFunctionType.Transfer:
                        yield return BuildUMPatternFromTo(Resources.UMTransferPattern, message);
                        break;

                    default:
                        continue;
                }
            }
        }

        private UMMessageDTO BuildCustomerSettingsMessage(UMMessageItem message)
        {
            var builder = new StringBuilder(
                string.Format(Resources.UMCustomerSettingsPattern, message.BeginTime.ToLongDateWithSlash(), message.EndTime.ToLongDateWithSlash()));
            builder
                .Replace("_SettingType_", settingTypeResource.GetString($"_{message.SettingType}"))
                .Replace("_Product_", GetProductOrSportType(message.ProductId, message.SportType));

            return new UMMessageDTO(BitConverter.ToInt64(message.Time_Stamp, 0), builder.ToString());
        }

        private string GetProductOrSportType(int productId, int sportTypeId)
        {
            if (sportTypeId > 0)
            {
                return GetSportName(sportTypeId);
            }

            return productId == 0 ? Resources.AllProducts : GetProductName(productId);
        }

        private static UMMessageDTO BuildUMPatternFromTo(string pattern, UMMessageItem message)
            => new(
                BitConverter.ToInt64(message.Time_Stamp, 0),
                string.Format(
                    pattern,
                    message.BeginTime.ToLongDateWithSlash(),
                    message.EndTime.ToLongDateWithSlash()));

        public string GetSportName(int sportTypeId)
            => remotingRefService.GetSportName(sportTypeId.ToString(), LanguageCode, null)
            ?? remotingRefService.GetResourceDataByRefIdAndResourceId("DummySport", sportTypeId.ToString(), LanguageCode, string.Empty);

        public string GetProductName(int productId)
            => remotingRefService.GetResourceDataByRefIdAndResourceId(
                "Product",
                productId.ToString(),
                LanguageCode,
                string.Empty);

        private static string LanguageCode
            => Thread.CurrentThread.CurrentCulture.Name;
    }
}