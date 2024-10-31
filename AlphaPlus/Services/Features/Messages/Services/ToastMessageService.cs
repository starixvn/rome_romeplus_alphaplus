using AlphaPlus.Features.Messages.API.Criteria.ToastMessage;
using AlphaPlus.Features.Messages.API.Models.ToastMessage;
using AlphaPlus.Features.Messages.API.Params.ToastMessage;
using AlphaPlus.Core.Models;
using Fanex.Data.Repository;

namespace AlphaPlus.Features.Messages.API.Services
{
    public interface ToastMessageService
    {
        Task<IEnumerable<ToastMessageModel>> Get(GetToastMessagesParams messageParams);
        Task<Result> MarkRead(MarkReadMessageParams messageParams);
    }

    public class ToastMessageServiceImpl : ToastMessageService
    {
        private readonly IDynamicRepository dynamicRepository;

        public ToastMessageServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IEnumerable<ToastMessageModel>> Get(GetToastMessagesParams messageParams)
        {
            var criteria = new GetToastMessagesCriteria(messageParams.CustId, messageParams.SubAccId, messageParams.Language, messageParams.LastToastId);
            var models = await dynamicRepository.FetchAsync<ToastMessageModel>(criteria);

            var resultMessage = ProritizeMessage(models);

            if (resultMessage == null)
            {
                return Enumerable.Empty<ToastMessageModel>();
            }

            return new List<ToastMessageModel> { resultMessage };
        }

        public async Task<Result> MarkRead(MarkReadMessageParams messageParams)
        {
            var criteria = new MarkReadMessageCriteria(messageParams.Id, messageParams.TargetId, messageParams.SubAccId);
            await dynamicRepository.ExecuteAsync(criteria);

            return new Result(criteria.ErrCode, criteria.ErrMsg);
        }

        public static ToastMessageModel? ProritizeMessage(IEnumerable<ToastMessageModel> messages)
        {
            var result = messages.OrderByDescending(m => m.CreatedTime).FirstOrDefault();

            return result;
        }
    }
}