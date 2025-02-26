using AlphaPlus.Features.Messages.API.Criteria.Message;
using Fanex.Data.Repository;
using AlphaPlus.Features.Messages.API.Models.Message;

namespace AlphaPlus.Features.Messages.API.Services
{
    public interface PortalMessageService
    {
        Task<IEnumerable<PortalMessage>> GetPortalMessages(
            int custId,
            string parentSites,
            int totalMessage,
            bool isTest);
        Task<bool> MarkMessageAsRead(int custId, int messageId, bool isInternal);
    }

    public class PortalMessageServiceImpl : PortalMessageService
    {
        private readonly IDynamicRepository dynamicRepository;

        public PortalMessageServiceImpl(
            IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<PortalMessage>> GetPortalMessages(
            int custId,
            string parentSites,
            int totalMessage,
            bool isTest)
        {
            var criteria = new GetPortalMessagesCriteria(custId, parentSites, totalMessage, isTest);

            var message = await dynamicRepository.FetchAsync<PortalMessage>(criteria);

            return message;
        }

        public async Task<bool> MarkMessageAsRead(int custId, int messageId, bool isInternal)
        {
            var command = new MarkMessageAsReadCommand(custId, messageId.ToString());

            if (!command.IsValid() || isInternal)
            {
                return false;
            }

            await dynamicRepository.ExecuteAsync(command);

            return true;
        }
    }
}