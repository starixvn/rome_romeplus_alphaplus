using AlphaPlus.Settings.MemberInfo.Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AlphaPlus.Settings.Site.Features.MemberInfo.Features.Commission;

public class MemberCommissionViewModel(
    MemberCommissionQuery message,
    IEnumerable<Product> products)
{
    public IEnumerable<SelectListItem> Products
        => products.Select(p => new SelectListItem(p.Name, p.Id.ToString(), p.Id == message.Id));
}