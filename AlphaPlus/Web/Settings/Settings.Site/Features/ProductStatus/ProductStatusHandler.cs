using AlphaPlus.Core.Models;
using AlphaPlus.Settings.Site.Features.ProductStatus.Models;
using AlphaPlus.Settings.Site.Features.ProductStatus.Params;
using AlphaPlus.Settings.Site.Features.ProductStatus.Requests;
using AlphaPlus.Settings.Site.Features.ProductStatus.ViewModels;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.ProductStatus
{
    public class ProductStatusHandler(IProductStatusService productStatusService) :
        IRequestHandler<ProductStatusQuery, ProductStatusViewModel>,
        IRequestHandler<ProductStatusCommand, IEnumerable<Result>>
    {
        private readonly IProductStatusService productStatusService = productStatusService;

        public async Task<ProductStatusViewModel> Handle(ProductStatusQuery request, CancellationToken cancellationToken)
        {
            var model = await productStatusService.GetProductStatus(new ProductStatusAuthorizationParam(
                request.IsGroup,
                request.ProductId,
                request.CustId,
                request.CustRole,
                request.UplineId,
                request.UplineLevel,
                request.OfferedProductGroupIds));

            return ToProductStatusViewModel(request, model);
        }

        private static ProductStatusViewModel ToProductStatusViewModel(ProductStatusQuery message, IEnumerable<ProductStatusModel> productStatus)
            => new(
                message.ProductId,
                message.CustId,
                message.CustName,
                Enumeration.FromValue<UserLevel>(message.CustRoleId),
                productStatus,
                message.UplineLevel,
                message.IsViewOnly,
                message.IsGroup,
                message.AgentId);

        public Task<IEnumerable<Result>> Handle(ProductStatusCommand request, CancellationToken cancellationToken)
        {
            var commandParam = ToUpdateProductStatusParam(request);

            return productStatusService.UpdateProductStatus(commandParam);
        }

        private static ProductStatusCommandParam ToUpdateProductStatusParam(ProductStatusCommand message)
            => new(
                message.IsGroup,
                message.ProductId,
                message.CustId,
                message.CustRole,
                message.UplineId,
                message.UplineLevel,
                message.ActorId,
                message.ActorName,
                message.SubAccId,
                message.LangKey,
                message.IP,
                message.BetTypeStatus.Select(status => new BetTypeStatusParam(status.SportType, status.BetType, status.IsDisabled)),
                message.Language,
                message.OfferedProductGroupIds);
    }
}