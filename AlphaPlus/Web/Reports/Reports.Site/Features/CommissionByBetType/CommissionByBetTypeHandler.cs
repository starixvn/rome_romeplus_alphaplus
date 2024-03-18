// using AlphaPlus.Reports.Features.CommissionByBetType;
// using AlphaPlus.Reports.Features.Shared.Extensions;
// using AlphaPlus.Reports.Site.Features.Shared;
// using MediatR;

// namespace AlphaPlus.Reports.Site.Features.CommissionByBetType;

// public class CommissionByBetTypeHandler(ICommissionByBetTypeService commissionByBetTypeService) :
//     BaseHandler,
//     IRequestHandler<CommissionByBetTypeQuery, CommissionByBetTypeViewModel>
// {
//     public async Task<CommissionByBetTypeViewModel> Handle(CommissionByBetTypeQuery message, CancellationToken cancellationToken)
//     {
//         var param = new CommissionByBetTypeParam(
//             message.CustLevel,
//             message.CustId,
//             message.FromDate.GetGreaterDate(message.LimitedDateToShowInfo),
//             message.ToDate,
//             message.Site);

//         var commissions = await commissionByBetTypeService.GetCommissionByBetTypes(param);
//         if (commissions.Count() > 1)
//         {
//             commissions = commissions.OrderBy(message.SortPropertyName, message.IsDescendingSort);
//         }

//         var total = new CommissionByBetTypeModel();
//         commissions.ToList().ForEach(item => total += item);

//         return new CommissionByBetTypeViewModel(
//             message,
//             commissions,
//             total,
//             BuildBreadcrumbs(message));
//     }
// }
