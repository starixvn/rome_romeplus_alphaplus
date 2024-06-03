using AlphaPlus.Settings.Site.Features.Shared;

namespace AlphaPlus.Settings.Site.Features.Customer.Requests
{
    public class EditCustomerQuery : BaseQuery
    {
        public byte RoleId { get; set; }

        public int ProductGroupId { get; set; }
    }
}