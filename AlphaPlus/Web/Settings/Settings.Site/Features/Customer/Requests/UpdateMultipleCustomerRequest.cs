using AlphaPlus.Settings.Site.Features.Shared;

namespace AlphaPlus.Settings.Site.Features.Customer.Requests
{
    public class UpdateMultipleCustomerRequest : BaseQuery
    {
        public string ProductGroups { get; set; }

        public string CustomerIds { get; set; }

        public string CustomerNames { get; set; }

        public byte RoleId { get; set; }

        public byte UplineRoleId { get; set; }
    }
}