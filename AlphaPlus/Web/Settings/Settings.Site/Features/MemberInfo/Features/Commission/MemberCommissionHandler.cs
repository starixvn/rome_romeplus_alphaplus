using AlphaPlus.Settings.MemberInfo.API.Features.Shared;
using AlphaPlus.Settings.MemberInfo.Domain.Configurations;
using MediatR;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.Commission;

public class MemberCommissionHandler(IProductService productService)
    : IRequestHandler<MemberCommissionQuery, MemberCommissionViewModel>
{
    private readonly IProductService productService = productService;

    public async Task<MemberCommissionViewModel> Handle(MemberCommissionQuery message, CancellationToken cancellationToken)
    {
        var products = await productService.GetCommProducts(message.ViewerId, AppSettings.SettingStatus);

        return new MemberCommissionViewModel(message, products);
    }
}