using System.Text;
using AlphaPlus.Core.Models;
using AlphaPlus.Shared.AppResources.Features.Messages;
using AlphaPlus.Features.Messages.API.Models.Feedback;
using AlphaPlus.Features.Messages.API.Params.Feedback;
using AlphaPlus.Features.Messages.API.Services;
using AlphaPlus.Features.Site.Messages.Requests.Feedback;
using AlphaPlus.Features.Site.Messages.ViewModels.Feedback;
using MediatR;

namespace AlphaPlus.Features.Site.Messages.Handlers
{
    public class FeedbackHandler :
        IRequestHandler<CheckExistsFeedbackQuery, CheckExistsFeedbackViewModel>,
        IRequestHandler<GetFunctionFeedbackQuery, FeedbackFunctionViewModel>,
        IRequestHandler<AddFeedbackQuery, Result>
    {
        private readonly FeedbackService feedbackService;

        public FeedbackHandler(FeedbackService feedbackService)
        {
            this.feedbackService = feedbackService;
        }

        public async Task<CheckExistsFeedbackViewModel> Handle(CheckExistsFeedbackQuery message, CancellationToken cancellation)
        {
            string functionName = string.Empty;
            var isExist = await feedbackService.CheckExistsAsync(message.CustId, message.SubAccId, message.Id);

            if (!isExist)
            {
                var feedback = await feedbackService.GetFeedbackFunctionAsync(message.Id);
                var feedbackFunction = feedback.Item1.FirstOrDefault();

                if (feedbackFunction != null)
                {
                    functionName = string.Format(Resources.FeedbackOnFunctionTitle, feedbackFunction.GetFunctionName(message.Language));
                }
            }

            return new CheckExistsFeedbackViewModel(isExist, functionName);
        }

        public async Task<FeedbackFunctionViewModel> Handle(GetFunctionFeedbackQuery message, CancellationToken cancellation)
        {
            var feedback = await feedbackService.GetFeedbackFunctionAsync(message.Id);
            var feedbackFunction = feedback.Item1.FirstOrDefault() ?? new FeedbackFunction();
            var feedbackAreas = feedback.Item2;

            return new FeedbackFunctionViewModel(
                feedbackFunction.FuncId,
                feedbackFunction.GetFunctionName(message.Language),
                feedbackAreas.Select(a => new FeedbackAreaViewModel(a.AreaId, a.GetAreaName(message.Language))));
        }

        public async Task<Result> Handle(AddFeedbackQuery message, CancellationToken cancellation)
        {
            if (message.IsValid())
            {
                var xmlFeedback = message.FunctionId.HasValue && message.Areas?.Any() == true
                   ? BuildXMLFeedback(message)
                   : null;
                await feedbackService.AddFeedbackAsync(new AddFeedbackParam(
                    message.CustId,
                    message.SubAccId,
                    message.Message,
                    message.Rate,
                    message.FunctionId,
                    xmlFeedback));

                return new Result { Message = Resources.Feedback_Success };
            }

            return new Result(1, message.IsInternal ? Resources.NotAuthorized : Resources.Feedback_InvalidParams);
        }

        private static string BuildXMLFeedback(AddFeedbackQuery message)
        {
            var xmlBuilder = new StringBuilder();
            xmlBuilder.Append("<Root>");

            foreach (var item in message.Areas)
            {
                xmlBuilder
                    .Append("<Feedback CustID=\"")
                    .Append(message.CustId)
                    .Append("\" SubAccID=\"")
                    .Append(message.SubAccId)
                    .Append("\" FuncID=\"")
                    .Append(message.FunctionId)
                    .Append("\" AreaID=\"")
                    .Append(item.AreaId)
                    .Append("\" Rate=\"")
                    .Append(item.Rate)
                    .Append("\"/>");
            }

            xmlBuilder.Append("</Root>");

            return xmlBuilder.ToString();
        }
    }
}