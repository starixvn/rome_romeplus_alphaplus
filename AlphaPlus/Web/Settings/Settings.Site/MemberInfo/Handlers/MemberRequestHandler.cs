namespace AlphaPlus.Settings.Site.Handlers.MemberRequest
{
    using System.Collections.Generic;
    using System.Linq;
    using AlphaPlus.Settings.MemberInfo.API.Services.MemberRequest;
    using AlphaPlus.Settings.MemberInfo.Domain.Models;
    using AlphaPlus.Settings.MemberInfo.Domain.Models.MemberRequests;
    using AlphaPlus.Settings.MemberInfo.Domain.Params;
    using MediatR;
    using Requests.MemberRequest;
    using ViewModels;
    using ViewModels.MemberRequest;

    public class MemberRequestHandler :
        IRequestHandler<ProductQuery, ProductsViewModel>,
        IRequestHandler<MemberRequestsQuery, MemberRequestViewModel>
    {
        private readonly MemberRequestService service;

        public MemberRequestHandler(MemberRequestService service)
        {
            this.service = service;
        }

        public  async Task<ProductsViewModel> Handle(ProductQuery message, CancellationToken cancellation)
        {
            var products = service.GetPlayForFunProduct(message.CustId);
            var viewModel = new ProductsViewModel(
                message.ViewerId,
                message.UserSelectedProductId,
                BuildProductsViewModel(new List<int> { message.UserSelectedProductId }, products, message));

            return viewModel;
        }

        public  async Task<MemberRequestViewModel> Handle(MemberRequestsQuery message, CancellationToken cancellation)
        {
            var param = new MemberRequestsParam(message.CustId, message.UserSelectedProductId);
            var memberRequestData = service.GetMemberRequest(param);

            var viewModel = new MemberRequestViewModel(
                memberRequestData,
                message.UserSelectedProductId,
                message.DisabledUserSelectedProduct,
                message.CustId,
                message.CanUpdateAccount);

            return viewModel;
        }

        protected internal IEnumerable<ProductViewModel> BuildProductsViewModel(
            IEnumerable<int> userSelectedProductIds,
            IEnumerable<Product> products,
            ProductQuery message)
        {
            if (products == null
                || !products.Any())
            {
                return Enumerable.Empty<ProductViewModel>();
            }

            return products.Select(product =>
            {
                var isDisable = !service.IsSync(product.Id) || product.IsDisable;
                var setting = isDisable
                ? new GetSettingResult(false, 0, string.Empty)
                : service.GetSetting(new ProductSettingParam(product.Id, message.CustId, message.CustName));

                var productSelectListItem = new ProductViewModel(
                    product.Name,
                    product.Id.ToString(),
                    userSelectedProductIds.Contains(product.Id),
                    isDisable,
                    setting);

                return productSelectListItem;
            });
        }
    }
}