using AlphaPlus.Settings.Site.Features.Shared;

namespace AlphaPlus.Settings.Site.Features.Customer.Requests;

public class UpdateCashCustomerInfoRequest : BaseQuery
{
    public bool IsEnableCash { get; set; }
}