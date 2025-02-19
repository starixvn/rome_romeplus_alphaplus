using AlphaPlus.Features.Messages.API.Constants;
using AlphaPlus.Features.Messages.API.Criteria.Message;
using AlphaPlus.Features.Messages.API.Models.Message;
using AlphaPlus.Features.Messages.Utils;
using Fanex.Data.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace AlphaPlus.Features.Messages.API.Services
{
    public interface MarqueeMessageService
    {
        GeneralMarqueeMessage GetLatestPublicMessage(byte roleId, string languageKey, bool isTest, string customerSite);

        GeneralMarqueeMessage GetLatestPrivateMessage(int custId, string languageKey, bool isTest);
    }

    public class MarqueeMessageServiceImpl : MarqueeMessageService
    {
        private const int CachedSecond = 30;
        private readonly IDynamicRepository dynamicRepository;
        private readonly IMemoryCache cacheService;

        public MarqueeMessageServiceImpl(IDynamicRepository dynamicRepository, IMemoryCache cacheService)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheService = cacheService;
        }

        public GeneralMarqueeMessage GetLatestPublicMessage(byte roleId, string languageKey, bool isTest, string customerSite)
        {
            var siteId = MessageConstants.ParentSitesByLevel[roleId];

            var message = cacheService.GetOrCreate(
                $"{DataBaseMessageType.Others}_{siteId}_{customerSite}",
                cacheEntry =>
                {
                    cacheEntry.SetAbsoluteExpiration(TimeSpan.FromSeconds(CachedSecond));
                    return dynamicRepository.Get<MarqueeMessage>(new GetLatestPublicMessageCriteria(DataBaseMessageType.Others, siteId, isTest, customerSite));
                });

            return new GeneralMarqueeMessage(message.Id, message.GetContentByKey(languageKey), message.BetTransId, message.IsBatch, message.IsSticky, message.MsgType);
        }

        public GeneralMarqueeMessage GetLatestPrivateMessage(int custId, string languageKey, bool isTest)
        {
            var generalMessage = new GeneralMarqueeMessage();
            var hasLatestPrivateMessage = HasLatestPrivateMessage(custId, isTest);
            if (hasLatestPrivateMessage)
            {
                var message = dynamicRepository.Get<MarqueeMessage>(new GetLatestPrivateMessageCriteria(custId, isTest));
                if (message != null)
                {
                    var rawContent = message.GetContentByKey(languageKey);
                    generalMessage = new GeneralMarqueeMessage(
                        message.Id,
                        MessageFormatter.MarkBoldText(rawContent, languageKey),
                        message.BetTransId,
                        message.IsBatch,
                        message.IsSticky,
                        message.MsgType);
                }
            }

            return generalMessage;
        }

        private bool HasLatestPrivateMessage(int custId, bool isTest)
        {
            var id = dynamicRepository.Get<int>(new HasLatestPrivateMessageCriteria(custId, isTest));

            return id > 0;
        }
    }
}