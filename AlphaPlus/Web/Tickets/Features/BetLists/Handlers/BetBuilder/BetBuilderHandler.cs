// using AlphaPlus.Tickets.Site.Features.BetLists.Requests.BetBuilder;
// using Fanex.BetDetails.BetBuilder.Criteria;
// using Fanex.BetDetails.BetBuilder.Services;
// using Fanex.BetDetails.BetBuilder.ViewModels;
// using MediatR;

// namespace AlphaPlus.Tickets.Site.Features.BetLists.Handlers.BetBuilder;

// public class BetBuilderHandler(IBetBuilderService betBuilderService)
//     : IRequestHandler<BetBuilderDetailQuery, BetBuilderDetailViewModel>
// {
//     private readonly IBetBuilderService betBuilderService = betBuilderService;

//     public Task<BetBuilderDetailViewModel> Handle(BetBuilderDetailQuery message, CancellationToken cancellationToken)
//     {
//         var viewModel = betBuilderService.GetBetListDetail(new GetBetBuilderDetailParameter
//         {
//             CustomerId = message.CustomerId,
//             TransactionId = message.TransactionId,
//             Status = message.Status
//         });

//         return Task.FromResult(viewModel);
//     }
// }
