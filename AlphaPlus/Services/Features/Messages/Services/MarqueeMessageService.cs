namespace AlphaPlus.Features.Messages.API.Services
{
    using AlphaPlus.Features.Messages.Utils;
    using Constants;
    using Criteria.Message;
    using Fanex.Caching;
    using Fanex.Data.Repository;
    using Models.Message;
    using System;

    public interface MarqueeMessageService
    {
        GeneralMarqueeMessage GetLatestPublicMessage(byte roleId, string languageKey, bool isTest, string customerSite);

        GeneralMarqueeMessage GetLatestPrivateMessage(int custId, string languageKey, bool isTest);
    }

    public class MarqueeMessageServiceImpl : MarqueeMessageService
    {
        private const int CachedSecond = 30;
        private readonly IDynamicRepository dynamicRepository;
        private readonly ICacheService cacheService;

        public MarqueeMessageServiceImpl(IDynamicRepository dynamicRepository, ICacheService cacheService)
        {
            this.dynamicRepository = dynamicRepository;
            this.cacheService = cacheService;
        }

        public GeneralMarqueeMessage GetLatestPublicMessage(byte roleId, string languageKey, bool isTest, string customerSite)
        {
            var siteId = MessageConstants.ParentSitesByLevel[roleId];

            var message = cacheService.GetOrSet(
                $"{DataBaseMessageType.Others}_{siteId}_{customerSite}",
                () => dynamicRepository.Get<MarqueeMessage>(new GetLatestPublicMessageCriteria(DataBaseMessageType.Others, siteId, isTest, customerSite)),
                new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(CachedSecond)));

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