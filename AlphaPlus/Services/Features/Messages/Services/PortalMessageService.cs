namespace AlphaPlus.Features.Messages.API.Services
{
    using System.Collections.Generic;
    using Criteria.Message;
    using Fanex.Data.Repository;
    using Models.Message;

    public interface PortalMessageService
    {
        IEnumerable<PortalMessage> GetPortalMessages(
            int custId,
            string parentSites,
            int totalMessage,
            bool isTest);
        bool MarkMessageAsRead(int custId, int messageId, bool isInternal);
    }

    public class PortalMessageServiceImpl : PortalMessageService
    {
        private readonly IDynamicRepository dynamicRepository;

        public PortalMessageServiceImpl(
            IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public IEnumerable<PortalMessage> GetPortalMessages(
            int custId,
            string parentSites,
            int totalMessage,
            bool isTest)
        {
            var criteria = new GetPortalMessagesCriteria(custId, parentSites, totalMessage, isTest);

            var message = dynamicRepository.Fetch<PortalMessage>(criteria);

            return message;
        }

        public bool MarkMessageAsRead(int custId, int messageId, bool isInternal)
        {
            var command = new MarkMessageAsReadCommand(custId, messageId.ToString());

            if (!command.IsValid() || isInternal)
            {
                return false;
            }

            dynamicRepository.Execute(command);

            return true;
        }
    }
}