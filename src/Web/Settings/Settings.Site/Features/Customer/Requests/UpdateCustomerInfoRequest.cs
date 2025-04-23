using AlphaPlus.Settings.Site.Features.Shared;

namespace AlphaPlus.Settings.Site.Features.Customer.Requests;

public class UpdateCustomerInfoRequest : BaseQuery
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Phone { get; set; }

    public string MobilePhone { get; set; }

    public string Email { get; set; }

    public string Fax { get; set; }
}